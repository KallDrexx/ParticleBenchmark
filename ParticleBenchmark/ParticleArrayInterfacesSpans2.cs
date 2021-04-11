using System;
using System.Collections.Generic;
using System.Numerics;

namespace ParticleBenchmark
{
    /// <summary>
    /// Particles made up of arrays of individual properties surfaced by a dictionary of spans.  Each modifier is
    /// defined in interfaces, each modifier runs for all particles before the next modifier is run 
    /// </summary>
    public static class ParticleArrayInterfacesSpans2
    {
        public class ParticleCollection
        {
            public Dictionary<string, int[]> IntProperties = new();
            public Dictionary<string, byte[]> ByteProperties = new();
            public Dictionary<string, float[]> FloatProperties = new();
            public Dictionary<string, Vector2[]> Vector2Properties = new();

            public ParticleCollection()
            {
                var intKeys = new[] {"AltitudeBounceCount"};
                var byteKeys = new[] {"TextureSectionIndex", "InitialRed", "InitialGreen", "InitialBlue", "InitialAlpha"};
                var floatKeys = new[]
                {
                    "TimeAlive", "RotationInRadians", "RotationalVelocityInRadians", "CurrentRed", "CurrentGreen",
                    "CurrentBlue", "CurrentAlpha", "AltitudeVelocity", "Altitude"
                };

                var vectorKeys = new[] {"Size", "InitialSize", "Position", "ReferencePosition", "Velocity"};

                foreach (var key in intKeys)
                {
                    IntProperties[key] = new int[Program.ParticleCount];
                }
                
                foreach (var key in byteKeys)
                {
                    ByteProperties[key] = new byte[Program.ParticleCount];
                }
                
                foreach (var key in floatKeys)
                {
                    FloatProperties[key] = new float[Program.ParticleCount];
                }
                
                foreach (var key in vectorKeys)
                {
                    Vector2Properties[key] = new Vector2[Program.ParticleCount];
                }
            }

            public Span<int> GetIntValues(string property) => IntProperties[property].AsSpan();
            public Span<byte> GetByteValues(string property) => ByteProperties[property].AsSpan();
            public Span<float> GetFloatValues(string property) => FloatProperties[property].AsSpan();
            public Span<Vector2> GetVector2Values(string property) => Vector2Properties[property].AsSpan();
        }

        public class Emitter
        {
            public static float MaxParticleLifeTime { get; set; } = 5f;
            public static float SizeChange { get; set; } = 5f;
            public static float EndValue { get; set; } = 0f;
            public static float Drag { get; set; } = 0.1f;

            public readonly ParticleCollection Particles = new ParticleCollection();

            private readonly IModifier[] _modifiers;

            public Emitter(IModifier[] modifiers)
            {
                _modifiers = modifiers;
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    Particles.GetFloatValues("TimeAlive")[x] = 0;
                    Particles.GetFloatValues("RotationInRadians")[x] = 1;
                    Particles.GetVector2Values("Position")[x] = new Vector2(100, 100);
                    Particles.GetVector2Values("ReferencePosition")[x] = new Vector2(100, 100);
                    Particles.GetFloatValues("RotationalVelocityInRadians")[x] = 1f;
                    Particles.GetFloatValues("CurrentRed")[x] = 255;
                    Particles.GetFloatValues("CurrentGreen")[x] = 255;
                    Particles.GetFloatValues("CurrentBlue")[x] = 255;
                    Particles.GetFloatValues("CurrentAlpha")[x] = 255;
                    Particles.GetVector2Values("Size")[x] = Vector2.Zero;
                    Particles.GetVector2Values("InitialSize")[x] = new Vector2(32, 32);
                    Particles.GetVector2Values("Velocity")[x] = new Vector2(100, 100);
                    Particles.GetByteValues("TextureSectionIndex")[x] = 0;
                    Particles.GetByteValues("InitialAlpha")[x] = 255;
                    Particles.GetByteValues("InitialBlue")[x] = 255;
                    Particles.GetByteValues("InitialGreen")[x] = 255;
                    Particles.GetByteValues("InitialRed")[x] = 255;
                    Particles.GetFloatValues("Altitude")[x] = 0;
                    Particles.GetFloatValues("AltitudeVelocity")[x] = 0;
                    Particles.GetIntValues("AltitudeBounceCount")[x] = 0;
                }
            }

            public void Update(float timeSinceLastFrame)
            {
                var timeAlive = Particles.GetFloatValues("TimeAlive");
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    timeAlive[x] += timeSinceLastFrame;
                }
                
                foreach (var modifier in _modifiers)
                {
                    modifier.Modify(timeSinceLastFrame, Particles);
                }
            }
        }

        public interface IModifier
        {
            void Modify(float timeSinceLastFrame, ParticleCollection particles);
        }

        public class Modifier1 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                var textureSectionIndex = particles.GetByteValues("TextureSectionIndex");
                var timeAlive = particles.GetFloatValues("TimeAlive");
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    textureSectionIndex[x] = (byte) ((timeAlive[x] / Emitter.MaxParticleLifeTime) * 10);
                }
            }
        }

        public class Modifier2 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                var velocity = particles.GetVector2Values("Velocity");
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    velocity[x] += timeSinceLastFrame * new Vector2(Emitter.SizeChange, Emitter.SizeChange);
                }
            }
        }
        
        public class Modifier3 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                var size = particles.GetVector2Values("Size");
                
                for (var x = 0; x < Program.ParticleCount; x++)
                    size[x] += timeSinceLastFrame * new Vector2(Emitter.SizeChange, Emitter.SizeChange);
            }
        }
        
        public class Modifier4 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                var velocity = particles.GetVector2Values("Velocity");
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    velocity[x] -= Emitter.Drag * velocity[x] * timeSinceLastFrame;
                }
            }
        }
        
        public class Modifier5 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                var currentRed = particles.GetFloatValues("CurrentRed");
                var currentGreen = particles.GetFloatValues("CurrentGreen");
                var currentBlue = particles.GetFloatValues("CurrentBlue");
                var currentAlpha = particles.GetFloatValues("CurrentAlpha");
                var initialRed = particles.GetByteValues("InitialRed");
                var initialGreen = particles.GetByteValues("InitialGreen");
                var initialBlue = particles.GetByteValues("InitialBlue");
                var initialAlpha = particles.GetByteValues("InitialAlpha");
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    currentRed[x] -= (((initialRed[x] - Emitter.EndValue) / Emitter.MaxParticleLifeTime) *
                                                timeSinceLastFrame);
                    currentGreen[x] -= (((initialGreen[x] - Emitter.EndValue) / Emitter.MaxParticleLifeTime) *
                                                              timeSinceLastFrame);
                    currentBlue[x] -= (((initialBlue[x] - Emitter.EndValue) / Emitter.MaxParticleLifeTime) *
                                                              timeSinceLastFrame);
                    currentAlpha[x] -= (((initialAlpha[x] - Emitter.EndValue) / Emitter.MaxParticleLifeTime) *
                                                              timeSinceLastFrame);
                }
            }
        }
        
        public class Modifier6 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                var initialSize = particles.GetVector2Values("InitialSize");
                var size = particles.GetVector2Values("Size");
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    var width = (((initialSize[x].X - Emitter.EndValue) / Emitter.MaxParticleLifeTime) *
                                 timeSinceLastFrame);
                    var height = (((initialSize[x].Y - Emitter.EndValue) / Emitter.MaxParticleLifeTime) *
                                  timeSinceLastFrame);
                    size[x].X -= width;
                    size[x].Y -= height;
                }
            }
        }
        
        public class Modifier7 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                var velocity = particles.GetVector2Values("Velocity");
                var rotationInRadians = particles.GetFloatValues("RotationInRadians");
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    if (velocity[x] != Vector2.Zero)
                    {
                        rotationInRadians[x] = 
                            (float) Math.Atan2(velocity[x].Y, velocity[x].X);
                    }
                }
            }
        }
        
        public class Modifier8 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                var velocity = particles.GetVector2Values("Velocity");
                var position = particles.GetVector2Values("Position");
                var referencePosition = particles.GetVector2Values("ReferencePosition");
                var altitude = particles.GetFloatValues("Altitude");
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    referencePosition[x] += velocity[x] * timeSinceLastFrame;
                    position[x].X = referencePosition[x].X;
                    position[x].Y = referencePosition[x].Y + altitude[x];
                }
            }
        }

        public class Modifier9 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                var rotationInRadians = particles.GetFloatValues("RotationInRadians");
                var rotationalVelocityInRadians = particles.GetFloatValues("RotationalVelocityInRadians");
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    rotationInRadians[x] += rotationalVelocityInRadians[x] * timeSinceLastFrame;
                }
            }
        }
    }
}