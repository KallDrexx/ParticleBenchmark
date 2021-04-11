using System;
using System.Collections.Generic;
using System.Numerics;

namespace ParticleBenchmark
{
    /// <summary>
    /// Particles made up of arrays of individual properties surfaced by a dictionary of spans.  Each modifier is
    /// defined in interfaces, each modifier runs for all particles before the next modifier is run 
    /// </summary>
    public static class ParticleArrayInterfacesSpans
    {
        public class ParticleCollection
        {
            public byte[] TextureSectionIndex = new byte[Program.ParticleCount];
            public Vector2[] Size = new Vector2[Program.ParticleCount];
            public Vector2[] InitialSize = new Vector2[Program.ParticleCount];
            public Vector2[] Position = new Vector2[Program.ParticleCount];
            public Vector2[] ReferencePosition = new Vector2[Program.ParticleCount];
            public Vector2[] Velocity = new Vector2[Program.ParticleCount];
            public float[] TimeAlive = new float[Program.ParticleCount];
            public float[] RotationInRadians = new float[Program.ParticleCount];
            public float[] RotationalVelocityInRadians = new float[Program.ParticleCount];
            public byte[] InitialRed = new byte[Program.ParticleCount];
            public byte[] InitialGreen = new byte[Program.ParticleCount];
            public byte[] InitialBlue = new byte[Program.ParticleCount];
            public byte[] InitialAlpha = new byte[Program.ParticleCount];
            public float[] CurrentRed = new float[Program.ParticleCount];
            public float[] CurrentGreen = new float[Program.ParticleCount];
            public float[] CurrentBlue = new float[Program.ParticleCount];
            public float[] CurrentAlpha = new float[Program.ParticleCount];
            public float[] Altitude = new float[Program.ParticleCount];
            public float[] AltitudeVelocity = new float[Program.ParticleCount];
            public int[] AltitudeBounceCount = new int[Program.ParticleCount];
        }

        public class Emitter
        {
            private readonly Dictionary<string, Memory<byte>> _byteProperties = new();
            private readonly Dictionary<string, Memory<int>> _intProperties = new();
            private readonly Dictionary<string, Memory<float>> _floatProperties = new();
            private readonly Dictionary<string, Memory<Vector2>> _vector2Properties = new();
            
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
                    Particles.TimeAlive[x] = 0;
                    Particles.RotationInRadians[x] = 1;
                    Particles.Position[x] = new Vector2(100, 100);
                    Particles.ReferencePosition[x] = new Vector2(100, 100);
                    Particles.RotationalVelocityInRadians[x] = 1f;
                    Particles.CurrentRed[x] = 255;
                    Particles.CurrentGreen[x] = 255;
                    Particles.CurrentBlue[x] = 255;
                    Particles.CurrentAlpha[x] = 255;
                    Particles.Size[x] = Vector2.Zero;
                    Particles.InitialSize[x] = new Vector2(32, 32);
                    Particles.Velocity[x] = new Vector2(100, 100);
                    Particles.TextureSectionIndex[x] = 0;
                    Particles.InitialAlpha[x] = 255;
                    Particles.InitialBlue[x] = 255;
                    Particles.InitialGreen[x] = 255;
                    Particles.InitialRed[x] = 255;
                    Particles.Altitude[x] = 0;
                    Particles.AltitudeVelocity[x] = 0;
                    Particles.AltitudeBounceCount[x] = 0;
                }

                _byteProperties["TextureSectionIndex"] = Particles.TextureSectionIndex.AsMemory();
                _vector2Properties["Size"] = Particles.Size.AsMemory();
                _vector2Properties["InitialSize"] = Particles.InitialSize.AsMemory();
                _vector2Properties["Position"] = Particles.Position.AsMemory();
                _vector2Properties["ReferencePosition"] = Particles.ReferencePosition.AsMemory();
                _vector2Properties["Velocity"] = Particles.Velocity.AsMemory();
                _floatProperties["TimeAlive"] = Particles.TimeAlive.AsMemory();
                _floatProperties["RotationInRadians"] = Particles.RotationInRadians.AsMemory();
                _floatProperties["RotationalVelocityInRadians"] = Particles.RotationalVelocityInRadians.AsMemory();
                _byteProperties["InitialRed"] = Particles.InitialRed.AsMemory();
                _byteProperties["InitialGreen"] = Particles.InitialGreen.AsMemory();
                _byteProperties["InitialBlue"] = Particles.InitialBlue.AsMemory();
                _byteProperties["InitialAlpha"] = Particles.InitialAlpha.AsMemory();
                _floatProperties["CurrentRed"] = Particles.CurrentRed.AsMemory();
                _floatProperties["CurrentGreen"] = Particles.CurrentGreen.AsMemory();
                _floatProperties["CurrentBlue"] = Particles.CurrentBlue.AsMemory();
                _floatProperties["CurrentAlpha"] = Particles.CurrentAlpha.AsMemory();
                _floatProperties["Altitude"] = Particles.Altitude.AsMemory();
                _floatProperties["AltitudeVelocity"] = Particles.AltitudeVelocity.AsMemory();
                _intProperties["AltitudeBounceCount"] = Particles.AltitudeBounceCount.AsMemory();
            }

            public void Update(float timeSinceLastFrame)
            {
                var timeAlive = _floatProperties["TimeAlive"].Span;
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    timeAlive[x] += timeSinceLastFrame;
                }
                
                foreach (var modifier in _modifiers)
                {
                    modifier.Modify(timeSinceLastFrame, _byteProperties, _intProperties, _floatProperties, _vector2Properties);
                }
            }
        }

        public interface IModifier
        {
            void Modify(float timeSinceLastFrame,
                IReadOnlyDictionary<string, Memory<byte>> byteProperties,
                IReadOnlyDictionary<string, Memory<int>> intProperties,
                IReadOnlyDictionary<string, Memory<float>> floatProperties,
                IReadOnlyDictionary<string, Memory<Vector2>> vector2Properties);
        }

        public class Modifier1 : IModifier
        {
            public void Modify(float timeSinceLastFrame,
                IReadOnlyDictionary<string, Memory<byte>> byteProperties,
                IReadOnlyDictionary<string, Memory<int>> intProperties,
                IReadOnlyDictionary<string, Memory<float>> floatProperties,
                IReadOnlyDictionary<string, Memory<Vector2>> vector2Properties)
            {
                var textureSectionIndex = byteProperties["TextureSectionIndex"].Span;
                var timeAlive = floatProperties["TimeAlive"].Span;
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    textureSectionIndex[x] = (byte) ((timeAlive[x] / Emitter.MaxParticleLifeTime) * 10);
                }
            }
        }

        public class Modifier2 : IModifier
        {
            public void Modify(float timeSinceLastFrame,
                IReadOnlyDictionary<string, Memory<byte>> byteProperties,
                IReadOnlyDictionary<string, Memory<int>> intProperties,
                IReadOnlyDictionary<string, Memory<float>> floatProperties,
                IReadOnlyDictionary<string, Memory<Vector2>> vector2Properties)
            {
                var velocity = vector2Properties["Velocity"].Span;
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    velocity[x] += timeSinceLastFrame * new Vector2(Emitter.SizeChange, Emitter.SizeChange);
                }
            }
        }
        
        public class Modifier3 : IModifier
        {
            public void Modify(float timeSinceLastFrame,
                IReadOnlyDictionary<string, Memory<byte>> byteProperties,
                IReadOnlyDictionary<string, Memory<int>> intProperties,
                IReadOnlyDictionary<string, Memory<float>> floatProperties,
                IReadOnlyDictionary<string, Memory<Vector2>> vector2Properties)
            {
                var size = vector2Properties["Size"].Span;
                for (var x = 0; x < Program.ParticleCount; x++)
                    size[x] += timeSinceLastFrame * new Vector2(Emitter.SizeChange, Emitter.SizeChange);
            }
        }
        
        public class Modifier4 : IModifier
        {
            public void Modify(float timeSinceLastFrame,
                IReadOnlyDictionary<string, Memory<byte>> byteProperties,
                IReadOnlyDictionary<string, Memory<int>> intProperties,
                IReadOnlyDictionary<string, Memory<float>> floatProperties,
                IReadOnlyDictionary<string, Memory<Vector2>> vector2Properties)
            {
                var velocity = vector2Properties["Velocity"].Span;
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    velocity[x] -= Emitter.Drag * velocity[x] * timeSinceLastFrame;
                }
            }
        }
        
        public class Modifier5 : IModifier
        {
            public void Modify(float timeSinceLastFrame,
                IReadOnlyDictionary<string, Memory<byte>> byteProperties,
                IReadOnlyDictionary<string, Memory<int>> intProperties,
                IReadOnlyDictionary<string, Memory<float>> floatProperties,
                IReadOnlyDictionary<string, Memory<Vector2>> vector2Properties)
            {
                var currentRed = floatProperties["CurrentRed"].Span;
                var currentGreen = floatProperties["CurrentGreen"].Span;
                var currentBlue = floatProperties["CurrentBlue"].Span;
                var currentAlpha = floatProperties["CurrentAlpha"].Span;
                var initialRed = byteProperties["InitialRed"].Span;
                var initialGreen = byteProperties["InitialGreen"].Span;
                var initialBlue = byteProperties["InitialBlue"].Span;
                var initialAlpha = byteProperties["InitialAlpha"].Span;
                
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
            public void Modify(float timeSinceLastFrame,
                IReadOnlyDictionary<string, Memory<byte>> byteProperties,
                IReadOnlyDictionary<string, Memory<int>> intProperties,
                IReadOnlyDictionary<string, Memory<float>> floatProperties,
                IReadOnlyDictionary<string, Memory<Vector2>> vector2Properties)
            {
                var initialSize = vector2Properties["InitialSize"].Span;
                var size = vector2Properties["Size"].Span;
                
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
            public void Modify(float timeSinceLastFrame,
                IReadOnlyDictionary<string, Memory<byte>> byteProperties,
                IReadOnlyDictionary<string, Memory<int>> intProperties,
                IReadOnlyDictionary<string, Memory<float>> floatProperties,
                IReadOnlyDictionary<string, Memory<Vector2>> vector2Properties)
            {
                var velocity = vector2Properties["Velocity"].Span;
                var rotationInRadians = floatProperties["RotationInRadians"].Span;
                
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
            public void Modify(float timeSinceLastFrame,
                IReadOnlyDictionary<string, Memory<byte>> byteProperties,
                IReadOnlyDictionary<string, Memory<int>> intProperties,
                IReadOnlyDictionary<string, Memory<float>> floatProperties,
                IReadOnlyDictionary<string, Memory<Vector2>> vector2Properties)
            {
                var velocity = vector2Properties["Velocity"].Span;
                var referencePosition = vector2Properties["ReferencePosition"].Span;
                var position = vector2Properties["Position"].Span;
                var altitude = floatProperties["Altitude"].Span;
                
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
            public void Modify(float timeSinceLastFrame,
                IReadOnlyDictionary<string, Memory<byte>> byteProperties,
                IReadOnlyDictionary<string, Memory<int>> intProperties,
                IReadOnlyDictionary<string, Memory<float>> floatProperties,
                IReadOnlyDictionary<string, Memory<Vector2>> vector2Properties)
            {
                var rotationInRadians = floatProperties["RotationInRadians"].Span;
                var rotationVelocityInRadians = floatProperties["RotationalVelocityInRadians"].Span;
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    rotationInRadians[x] += rotationVelocityInRadians[x] * timeSinceLastFrame;
                }
            }
        }
    }
}