using System.Collections.Generic;
using ComputeSharp;

namespace ParticleBenchmark
{
    /// <summary>
    /// Particles made up of arrays of individual properties surfaced by a dictionary of spans.  Each modifier is
    /// defined in interfaces, each modifier runs for all particles before the next modifier is run.  Modifiers
    /// utilize Compute Sharp for possible performance
    /// </summary>
    public class ComputeShaderParticles
    {
        public struct Particle
        {
            public bool IsAlive;
            public byte TextureSectionIndex;
            public float SizeX;
            public float SizeY;
            public float InitialSizeX;
            public float InitialSizeY;
            public float PositionX;
            public float PositionY;
            public float ReferencePositionX;
            public float ReferencePositionY;
            public float VelocityX;
            public float VelocityY;
            public float TimeAlive;
            public float RotationInRadians;
            public float RotationalVelocityInRadians;
            public byte InitialRed;
            public byte InitialGreen;
            public byte InitialBlue;
            public byte InitialAlpha;
            public float CurrentRed;
            public float CurrentGreen;
            public float CurrentBlue;
            public float CurrentAlpha;
            public float Altitude;
            public float AltitudeVelocity;
            public int AltitudeBounceCount;
        }
        
        public class Emitter
        {
            public static float MaxParticleLifeTime { get; set; } = 5f;
            public static float SizeChange { get; set; } = 5f;
            public static float EndValue { get; set; } = 0f;
            public static float Drag { get; set; } = 0.1f;

            public readonly Particle[] Particles = new Particle[Program.ParticleCount];
            private readonly ReadWriteBuffer<Particle> _particleBuffer;
            private readonly Shader _shader;

            public Emitter()
            {
                for (var x = 0; x < Particles.Length; x++)
                {
                    Particles[x] = new Particle
                    {
                        IsAlive = true,
                        TimeAlive = 0,
                        RotationInRadians = 1,
                        PositionX = 100,
                        PositionY = 100,
                        ReferencePositionX = 100,
                        ReferencePositionY = 100,
                        RotationalVelocityInRadians = 1f,
                        CurrentRed = 255,
                        CurrentGreen = 255,
                        CurrentBlue = 255,
                        CurrentAlpha = 255,
                        SizeX = 0,
                        SizeY = 0,
                        InitialSizeX = 32,
                        InitialSizeY = 32,
                        VelocityX = 100,
                        VelocityY = 100,
                        TextureSectionIndex = 0,
                        InitialAlpha = 255,
                        InitialBlue = 255,
                        InitialGreen = 255,
                        InitialRed = 255,
                        Altitude = 0,
                        AltitudeVelocity = 0,
                        AltitudeBounceCount = 0,
                    };
                }

                _particleBuffer = Gpu.Default.AllocateReadWriteBuffer<Particle>(Program.ParticleCount);

                _shader = new Shader(_particleBuffer, MaxParticleLifeTime, SizeChange, Drag, EndValue);
            }

            public void Update(float timeSinceLastFrame)
            {
                _particleBuffer.CopyFrom(Particles);
                
                Gpu.Default.For(Program.ParticleCount, _shader);
                
                _particleBuffer.CopyTo(Particles);
            }

            public readonly struct Shader : IComputeShader
            {
                public readonly ReadWriteBuffer<Particle> _buffer;
                public readonly float MaxParticleLifeTime;
                public readonly float SizeChange;
                public readonly float Drag;
                public readonly float EndValue;

                public Shader(ReadWriteBuffer<Particle> buffer,
                    float maxParticleLifeTime, float sizeChange, float drag, float endValue)
                {
                    _buffer = buffer;
                    MaxParticleLifeTime = maxParticleLifeTime;
                    SizeChange = sizeChange;
                    Drag = drag;
                    EndValue = endValue;
                }

                public void Execute()
                {
                    const float timeSinceLastFrame = 0.16f;
                    _buffer[ThreadIds.X].TimeAlive += timeSinceLastFrame;
                    _buffer[ThreadIds.X].TextureSectionIndex =
                        (byte) (_buffer[ThreadIds.X].TimeAlive / MaxParticleLifeTime);
                    _buffer[ThreadIds.X].VelocityX += timeSinceLastFrame * SizeChange;
                    _buffer[ThreadIds.X].VelocityY += timeSinceLastFrame * SizeChange;
                    _buffer[ThreadIds.X].VelocityX -= Drag * _buffer[ThreadIds.X].VelocityX * timeSinceLastFrame;
                    _buffer[ThreadIds.X].VelocityY -= Drag * _buffer[ThreadIds.X].VelocityY * timeSinceLastFrame;
                    _buffer[ThreadIds.X].SizeX += timeSinceLastFrame * SizeChange;
                    _buffer[ThreadIds.X].SizeY += timeSinceLastFrame * SizeChange;

                    _buffer[ThreadIds.X].CurrentRed = (_buffer[ThreadIds.X].InitialRed - EndValue) /
                        MaxParticleLifeTime * timeSinceLastFrame;

                    _buffer[ThreadIds.X].CurrentGreen = (_buffer[ThreadIds.X].InitialGreen - EndValue) /
                        MaxParticleLifeTime * timeSinceLastFrame;

                    _buffer[ThreadIds.X].CurrentBlue = (_buffer[ThreadIds.X].InitialBlue - EndValue) /
                        MaxParticleLifeTime * timeSinceLastFrame;

                    _buffer[ThreadIds.X].CurrentAlpha = (_buffer[ThreadIds.X].InitialAlpha - EndValue) /
                        MaxParticleLifeTime * timeSinceLastFrame;

                    _buffer[ThreadIds.X].SizeX -= (_buffer[ThreadIds.X].InitialSizeX - EndValue) /
                        MaxParticleLifeTime * timeSinceLastFrame;

                    _buffer[ThreadIds.X].SizeX -= (_buffer[ThreadIds.X].InitialSizeY - EndValue) /
                        MaxParticleLifeTime * timeSinceLastFrame;

                    _buffer[ThreadIds.X].RotationInRadians =
                        Hlsl.Atan2(_buffer[ThreadIds.X].VelocityY, _buffer[ThreadIds.X].VelocityX);

                    _buffer[ThreadIds.X].ReferencePositionX += _buffer[ThreadIds.X].VelocityX * timeSinceLastFrame;
                    _buffer[ThreadIds.X].ReferencePositionY += _buffer[ThreadIds.X].VelocityY * timeSinceLastFrame;
                    _buffer[ThreadIds.X].PositionX = _buffer[ThreadIds.X].ReferencePositionX;
                    _buffer[ThreadIds.X].PositionY = _buffer[ThreadIds.X].ReferencePositionY + _buffer[ThreadIds.X].Altitude;

                    _buffer[ThreadIds.X].RotationInRadians +=
                        _buffer[ThreadIds.X].RotationalVelocityInRadians * timeSinceLastFrame;
                }
            }
        }
    }
}