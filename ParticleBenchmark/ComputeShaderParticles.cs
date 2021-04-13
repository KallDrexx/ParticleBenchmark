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
        public class Emitter
        {
            public static float MaxParticleLifeTime { get; set; } = 5f;
            public static float SizeChange { get; set; } = 5f;
            public static float EndValue { get; set; } = 0f;
            public static float Drag { get; set; } = 0.1f;

            public readonly Dictionary<string, float[]> ParticleProperties = new();
            public readonly byte[] TextureSectionIndexArray;
            private readonly Shader _shader;
            
            private readonly ReadWriteBuffer<float> _timeAliveBuffer;
            private readonly ReadWriteBuffer<float> _rotationInRadiansBuffer;
            private readonly ReadWriteBuffer<float> _rotationalVelocityInRadiansBuffer;
            private readonly ReadWriteBuffer<float> _currentRedBuffer;
            private readonly ReadWriteBuffer<float> _currentGreenBuffer;
            private readonly ReadWriteBuffer<float> _currentBlueBuffer;
            private readonly ReadWriteBuffer<float> _currentAlphaBuffer;
            private readonly ReadWriteBuffer<float> _altitudeVelocityBuffer;
            private readonly ReadWriteBuffer<float> _altitudeBuffer;
            private readonly ReadWriteBuffer<float> _initialRedBuffer;
            private readonly ReadWriteBuffer<float> _initialGreenBuffer;
            private readonly ReadWriteBuffer<float> _initialBlueBuffer;
            private readonly ReadWriteBuffer<float> _initialAlphaBuffer;
            private readonly ReadWriteBuffer<float> _sizeXBuffer;
            private readonly ReadWriteBuffer<float> _initialSizeXBuffer;
            private readonly ReadWriteBuffer<float> _sizeYBuffer;
            private readonly ReadWriteBuffer<float> _initialSizeYBuffer;
            private readonly ReadWriteBuffer<float> _positionXBuffer;
            private readonly ReadWriteBuffer<float> _referencePositionXBuffer;
            private readonly ReadWriteBuffer<float> _velocityXBuffer;
            private readonly ReadWriteBuffer<float> _positionYBuffer;
            private readonly ReadWriteBuffer<float> _referencePositionYBuffer;
            private readonly ReadWriteBuffer<float> _velocityYBuffer;
            private readonly ReadWriteBuffer<byte> _textureSectionIndexBuffer;

            public Emitter()
            {
                var floatKeys = new[]
                {
                    "TimeAlive", "RotationInRadians", "RotationalVelocityInRadians", "CurrentRed", "CurrentGreen",
                    "CurrentBlue", "CurrentAlpha", "AltitudeVelocity", "Altitude", "InitialRed", "InitialGreen", 
                    "InitialBlue", "InitialAlpha", "SizeX", "InitialSizeX", "SizeY", "InitialSizeY",
                    "PositionX", "ReferencePositionX", "VelocityX", "PositionY", "ReferencePositionY", "VelocityY",
                };

                foreach (var key in floatKeys)
                {
                    ParticleProperties[key] = new float[Program.ParticleCount];
                }

                TextureSectionIndexArray = new byte[Program.ParticleCount];
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    ParticleProperties["TimeAlive"][x] = 0;
                    ParticleProperties["RotationInRadians"][x] = 1;
                    ParticleProperties["PositionX"][x] = 100;
                    ParticleProperties["PositionY"][x] = 100;
                    ParticleProperties["ReferencePositionX"][x] = 100;
                    ParticleProperties["ReferencePositionY"][x] = 100;
                    ParticleProperties["RotationalVelocityInRadians"][x] = 1f;
                    ParticleProperties["CurrentRed"][x] = 255;
                    ParticleProperties["CurrentGreen"][x] = 255;
                    ParticleProperties["CurrentBlue"][x] = 255;
                    ParticleProperties["CurrentAlpha"][x] = 255;
                    ParticleProperties["SizeX"][x] = 0;
                    ParticleProperties["InitialSizeX"][x] = 32;
                    ParticleProperties["SizeY"][x] = 0;
                    ParticleProperties["InitialSizeY"][x] = 32;
                    ParticleProperties["VelocityX"][x] = 100;
                    ParticleProperties["VelocityY"][x] = 100;
                    ParticleProperties["InitialAlpha"][x] = 255;
                    ParticleProperties["InitialBlue"][x] = 255;
                    ParticleProperties["InitialGreen"][x] = 255;
                    ParticleProperties["InitialRed"][x] = 255;
                    ParticleProperties["Altitude"][x] = 0;
                    ParticleProperties["AltitudeVelocity"][x] = 0;
                    TextureSectionIndexArray[x] = 0;
                }

                _timeAliveBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _rotationInRadiansBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _rotationalVelocityInRadiansBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _currentRedBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _currentGreenBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _currentBlueBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _currentAlphaBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _altitudeVelocityBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _altitudeBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _initialRedBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _initialGreenBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _initialBlueBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _initialAlphaBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _sizeXBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _initialSizeXBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _sizeYBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _initialSizeYBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _positionXBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _referencePositionXBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _velocityXBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _positionYBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _referencePositionYBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _velocityYBuffer= Gpu.Default.AllocateReadWriteBuffer<float>(Program.ParticleCount);
                _textureSectionIndexBuffer = Gpu.Default.AllocateReadWriteBuffer<byte>(Program.ParticleCount);

                _shader = new Shader(_timeAliveBuffer, _rotationInRadiansBuffer, _rotationalVelocityInRadiansBuffer,
                    _currentRedBuffer, _currentGreenBuffer, _currentBlueBuffer, _currentAlphaBuffer,
                    _altitudeVelocityBuffer,
                    _altitudeBuffer, _initialRedBuffer, _initialGreenBuffer, _initialBlueBuffer, _initialAlphaBuffer,
                    _sizeXBuffer, _initialSizeXBuffer, _sizeYBuffer, _initialSizeYBuffer, _positionXBuffer,
                    _referencePositionXBuffer, _velocityXBuffer, _positionYBuffer, _referencePositionYBuffer,
                    _velocityYBuffer, _textureSectionIndexBuffer, MaxParticleLifeTime, SizeChange, Drag, EndValue);
            }

            public void Update(float timeSinceLastFrame)
            {
                _timeAliveBuffer.CopyFrom(ParticleProperties["TimeAlive"]);
                _rotationInRadiansBuffer.CopyFrom(ParticleProperties["RotationInRadians"]);
                _rotationalVelocityInRadiansBuffer.CopyFrom(ParticleProperties["RotationalVelocityInRadians"]);
                _currentRedBuffer.CopyFrom(ParticleProperties["CurrentRed"]);
                _currentGreenBuffer.CopyFrom(ParticleProperties["CurrentGreen"]);
                _currentBlueBuffer.CopyFrom(ParticleProperties["CurrentBlue"]);
                _currentAlphaBuffer.CopyFrom(ParticleProperties["CurrentAlpha"]);
                _altitudeVelocityBuffer.CopyFrom(ParticleProperties["AltitudeVelocity"]);
                _altitudeBuffer.CopyFrom(ParticleProperties["Altitude"]);
                _initialRedBuffer.CopyFrom(ParticleProperties["InitialRed"]);
                _initialGreenBuffer.CopyFrom(ParticleProperties["InitialGreen"]);
                _initialBlueBuffer.CopyFrom(ParticleProperties["InitialBlue"]);
                _initialAlphaBuffer.CopyFrom(ParticleProperties["InitialAlpha"]);
                _sizeXBuffer.CopyFrom(ParticleProperties["SizeX"]);
                _initialSizeXBuffer.CopyFrom(ParticleProperties["InitialSizeX"]);
                _sizeYBuffer.CopyFrom(ParticleProperties["SizeY"]);
                _initialSizeYBuffer.CopyFrom(ParticleProperties["InitialSizeY"]);
                _positionXBuffer.CopyFrom(ParticleProperties["PositionX"]);
                _referencePositionXBuffer.CopyFrom(ParticleProperties["ReferencePositionX"]);
                _velocityXBuffer.CopyFrom(ParticleProperties["VelocityX"]);
                _positionYBuffer.CopyFrom(ParticleProperties["PositionY"]);
                _referencePositionYBuffer.CopyFrom(ParticleProperties["ReferencePositionY"]);
                _velocityYBuffer.CopyFrom(ParticleProperties["VelocityY"]);
                _textureSectionIndexBuffer.CopyFrom(TextureSectionIndexArray);
                
                Gpu.Default.For(Program.ParticleCount, _shader);
                
                _timeAliveBuffer.CopyTo(ParticleProperties["TimeAlive"]);
                _rotationInRadiansBuffer.CopyTo(ParticleProperties["RotationInRadians"]);
                _rotationalVelocityInRadiansBuffer.CopyTo(ParticleProperties["RotationalVelocityInRadians"]);
                _currentRedBuffer.CopyTo(ParticleProperties["CurrentRed"]);
                _currentGreenBuffer.CopyTo(ParticleProperties["CurrentGreen"]);
                _currentBlueBuffer.CopyTo(ParticleProperties["CurrentBlue"]);
                _currentAlphaBuffer.CopyTo(ParticleProperties["CurrentAlpha"]);
                _altitudeVelocityBuffer.CopyTo(ParticleProperties["AltitudeVelocity"]);
                _altitudeBuffer.CopyTo(ParticleProperties["Altitude"]);
                _initialRedBuffer.CopyTo(ParticleProperties["InitialRed"]);
                _initialGreenBuffer.CopyTo(ParticleProperties["InitialGreen"]);
                _initialBlueBuffer.CopyTo(ParticleProperties["InitialBlue"]);
                _initialAlphaBuffer.CopyTo(ParticleProperties["InitialAlpha"]);
                _sizeXBuffer.CopyTo(ParticleProperties["SizeX"]);
                _initialSizeXBuffer.CopyTo(ParticleProperties["InitialSizeX"]);
                _sizeYBuffer.CopyTo(ParticleProperties["SizeY"]);
                _initialSizeYBuffer.CopyTo(ParticleProperties["InitialSizeY"]);
                _positionXBuffer.CopyTo(ParticleProperties["PositionX"]);
                _referencePositionXBuffer.CopyTo(ParticleProperties["ReferencePositionX"]);
                _velocityXBuffer.CopyTo(ParticleProperties["VelocityX"]);
                _positionYBuffer.CopyTo(ParticleProperties["PositionY"]);
                _referencePositionYBuffer.CopyTo(ParticleProperties["ReferencePositionY"]);
                _velocityYBuffer.CopyTo(ParticleProperties["VelocityY"]);
                _textureSectionIndexBuffer.CopyTo(TextureSectionIndexArray);
            }
        }
    }
    
    public readonly struct Shader : IComputeShader
        {
            public readonly ReadWriteBuffer<float> TimeAliveBuffer;
            public readonly ReadWriteBuffer<float> RotationInRadiansBuffer;
            public readonly ReadWriteBuffer<float> RotationalVelocityInRadiansBuffer;
            public readonly ReadWriteBuffer<float> CurrentRedBuffer;
            public readonly ReadWriteBuffer<float> CurrentGreenBuffer;
            public readonly ReadWriteBuffer<float> CurrentBlueBuffer;
            public readonly ReadWriteBuffer<float> CurrentAlphaBuffer;
            public readonly ReadWriteBuffer<float> AltitudeVelocityBuffer;
            public readonly ReadWriteBuffer<float> AltitudeBuffer;
            public readonly ReadWriteBuffer<float> InitialRedBuffer;
            public readonly ReadWriteBuffer<float> InitialGreenBuffer;
            public readonly ReadWriteBuffer<float> InitialBlueBuffer;
            public readonly ReadWriteBuffer<float> InitialAlphaBuffer;
            public readonly ReadWriteBuffer<float> SizeXBuffer;
            public readonly ReadWriteBuffer<float> InitialSizeXBuffer;
            public readonly ReadWriteBuffer<float> SizeYBuffer;
            public readonly ReadWriteBuffer<float> InitialSizeYBuffer;
            public readonly ReadWriteBuffer<float> PositionXBuffer;
            public readonly ReadWriteBuffer<float> ReferencePositionXBuffer;
            public readonly ReadWriteBuffer<float> VelocityXBuffer;
            public readonly ReadWriteBuffer<float> PositionYBuffer;
            public readonly ReadWriteBuffer<float> ReferencePositionYBuffer;
            public readonly ReadWriteBuffer<float> VelocityYBuffer;
            public readonly ReadWriteBuffer<byte> TextureSectionIndexBuffer;
            public readonly float MaxParticleLifeTime;
            public readonly float SizeChange;
            public readonly float Drag;
            public readonly float EndValue;

            public Shader(ReadWriteBuffer<float> timeAliveBuffer, 
                ReadWriteBuffer<float> rotationInRadiansBuffer, 
                ReadWriteBuffer<float> rotationalVelocityInRadiansBuffer, 
                ReadWriteBuffer<float> currentRedBuffer, 
                ReadWriteBuffer<float> currentGreenBuffer, 
                ReadWriteBuffer<float> currentBlueBuffer, 
                ReadWriteBuffer<float> currentAlphaBuffer, 
                ReadWriteBuffer<float> altitudeVelocityBuffer, 
                ReadWriteBuffer<float> altitudeBuffer, 
                ReadWriteBuffer<float> initialRedBuffer, 
                ReadWriteBuffer<float> initialGreenBuffer, 
                ReadWriteBuffer<float> initialBlueBuffer, 
                ReadWriteBuffer<float> initialAlphaBuffer, 
                ReadWriteBuffer<float> sizeXBuffer, 
                ReadWriteBuffer<float> initialSizeXBuffer, 
                ReadWriteBuffer<float> sizeYBuffer, 
                ReadWriteBuffer<float> initialSizeYBuffer, 
                ReadWriteBuffer<float> positionXBuffer, 
                ReadWriteBuffer<float> referencePositionXBuffer, 
                ReadWriteBuffer<float> velocityXBuffer, 
                ReadWriteBuffer<float> positionYBuffer, 
                ReadWriteBuffer<float> referencePositionYBuffer, 
                ReadWriteBuffer<float> velocityYBuffer, 
                ReadWriteBuffer<byte> textureSectionIndexBuffer, 
                float maxParticleLifeTime, float sizeChange, float drag, float endValue)
            {
                TimeAliveBuffer = timeAliveBuffer;
                RotationInRadiansBuffer = rotationInRadiansBuffer;
                RotationalVelocityInRadiansBuffer = rotationalVelocityInRadiansBuffer;
                CurrentRedBuffer = currentRedBuffer;
                CurrentGreenBuffer = currentGreenBuffer;
                CurrentBlueBuffer = currentBlueBuffer;
                CurrentAlphaBuffer = currentAlphaBuffer;
                AltitudeVelocityBuffer = altitudeVelocityBuffer;
                AltitudeBuffer = altitudeBuffer;
                InitialRedBuffer = initialRedBuffer;
                InitialGreenBuffer = initialGreenBuffer;
                InitialBlueBuffer = initialBlueBuffer;
                InitialAlphaBuffer = initialAlphaBuffer;
                SizeXBuffer = sizeXBuffer;
                InitialSizeXBuffer = initialSizeXBuffer;
                SizeYBuffer = sizeYBuffer;
                InitialSizeYBuffer = initialSizeYBuffer;
                PositionXBuffer = positionXBuffer;
                ReferencePositionXBuffer = referencePositionXBuffer;
                VelocityXBuffer = velocityXBuffer;
                PositionYBuffer = positionYBuffer;
                ReferencePositionYBuffer = referencePositionYBuffer;
                VelocityYBuffer = velocityYBuffer;
                TextureSectionIndexBuffer = textureSectionIndexBuffer;
                MaxParticleLifeTime = maxParticleLifeTime;
                SizeChange = sizeChange;
                Drag = drag;
                EndValue = endValue;
            }

            public void Execute()
            {
                const float timeSinceLastFrame = 0.16f;
                TimeAliveBuffer[ThreadIds.X] += timeSinceLastFrame;
                TextureSectionIndexBuffer[ThreadIds.X] = (byte) (TimeAliveBuffer[ThreadIds.X] / MaxParticleLifeTime);
                VelocityXBuffer[ThreadIds.X] += timeSinceLastFrame * SizeChange;
                VelocityYBuffer[ThreadIds.X] += timeSinceLastFrame * SizeChange;
                VelocityXBuffer[ThreadIds.X] -= Drag * VelocityXBuffer[ThreadIds.X] * timeSinceLastFrame;
                VelocityYBuffer[ThreadIds.X] -= Drag * VelocityYBuffer[ThreadIds.X] * timeSinceLastFrame;
                SizeXBuffer[ThreadIds.X] += timeSinceLastFrame * SizeChange;
                SizeYBuffer[ThreadIds.X] += timeSinceLastFrame * SizeChange;

                CurrentRedBuffer[ThreadIds.X] = (InitialRedBuffer[ThreadIds.X] - EndValue) /
                    MaxParticleLifeTime * timeSinceLastFrame;
                
                CurrentGreenBuffer[ThreadIds.X] = (InitialGreenBuffer[ThreadIds.X] - EndValue) /
                    MaxParticleLifeTime * timeSinceLastFrame;
                
                CurrentBlueBuffer[ThreadIds.X] = (InitialBlueBuffer[ThreadIds.X] - EndValue) /
                    MaxParticleLifeTime * timeSinceLastFrame;
                
                CurrentAlphaBuffer[ThreadIds.X] = (InitialAlphaBuffer[ThreadIds.X] - EndValue) /
                    MaxParticleLifeTime * timeSinceLastFrame;

                SizeXBuffer[ThreadIds.X] -= (InitialSizeXBuffer[ThreadIds.X] - EndValue) /
                    MaxParticleLifeTime * timeSinceLastFrame;
                
                SizeXBuffer[ThreadIds.Y] -= (InitialSizeYBuffer[ThreadIds.X] - EndValue) /
                    MaxParticleLifeTime * timeSinceLastFrame;

                RotationInRadiansBuffer[ThreadIds.X] =
                    Hlsl.Atan2(VelocityYBuffer[ThreadIds.X], VelocityXBuffer[ThreadIds.X]);

                ReferencePositionXBuffer[ThreadIds.X] += VelocityXBuffer[ThreadIds.X] * timeSinceLastFrame;
                ReferencePositionYBuffer[ThreadIds.Y] += VelocityYBuffer[ThreadIds.X] * timeSinceLastFrame;
                PositionXBuffer[ThreadIds.X] = ReferencePositionXBuffer[ThreadIds.X];
                PositionYBuffer[ThreadIds.X] = ReferencePositionYBuffer[ThreadIds.X] + AltitudeBuffer[ThreadIds.X];

                RotationInRadiansBuffer[ThreadIds.X] +=
                    RotationalVelocityInRadiansBuffer[ThreadIds.X] * timeSinceLastFrame;
            }
        }
}