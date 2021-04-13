using System;
using System.Collections.Generic;
using System.Numerics;

namespace ParticleBenchmark
{
    /// <summary>
    /// Particles made up of arrays of individual properties surfaced by a dictionary of spans.  Each modifier is
    /// defined in interfaces, each modifier runs for all particles before the next modifier is run.  Modifiers
    /// utilize C# SIMD intrinsics for possible performance
    /// </summary>
    public static class ParticleArrayInterfacesSimd
    {
        private static int NearestMultiple(int number, int multiple) => ((number - 1) | (multiple - 1)) + 1 - multiple;
        
        public class ParticleCollection
        {
            public Dictionary<string, int[]> IntProperties = new();
            public Dictionary<string, byte[]> ByteProperties = new();
            public Dictionary<string, float[]> FloatProperties = new();

            public ParticleCollection()
            {
                var intKeys = new[] {"AltitudeBounceCount"};
                var byteKeys = new[] {"TextureSectionIndex", };
                var floatKeys = new[]
                {
                    "TimeAlive", "RotationInRadians", "RotationalVelocityInRadians", "CurrentRed", "CurrentGreen",
                    "CurrentBlue", "CurrentAlpha", "AltitudeVelocity", "Altitude", "InitialRed", "InitialGreen", 
                    "InitialBlue", "InitialAlpha", "SizeX", "InitialSizeX", "SizeY", "InitialSizeY",
                    "PositionX", "ReferencePositionX", "VelocityX", "PositionY", "ReferencePositionY", "VelocityY",
                };

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
            }

            public Span<int> GetIntValues(string property) => IntProperties[property].AsSpan();
            public Span<byte> GetByteValues(string property) => ByteProperties[property].AsSpan();
            public Span<float> GetFloatValues(string property) => FloatProperties[property].AsSpan();
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
                    Particles.GetFloatValues("PositionX")[x] = 100;
                    Particles.GetFloatValues("PositionY")[x] = 100;
                    Particles.GetFloatValues("ReferencePositionX")[x] = 100;
                    Particles.GetFloatValues("ReferencePositionY")[x] = 100;
                    Particles.GetFloatValues("RotationalVelocityInRadians")[x] = 1f;
                    Particles.GetFloatValues("CurrentRed")[x] = 255;
                    Particles.GetFloatValues("CurrentGreen")[x] = 255;
                    Particles.GetFloatValues("CurrentBlue")[x] = 255;
                    Particles.GetFloatValues("CurrentAlpha")[x] = 255;
                    Particles.GetFloatValues("SizeX")[x] = 0;
                    Particles.GetFloatValues("InitialSizeX")[x] = 32;
                    Particles.GetFloatValues("SizeY")[x] = 0;
                    Particles.GetFloatValues("InitialSizeY")[x] = 32;
                    Particles.GetFloatValues("VelocityX")[x] = 100;
                    Particles.GetFloatValues("VelocityY")[x] = 100;
                    Particles.GetByteValues("TextureSectionIndex")[x] = 0;
                    Particles.GetFloatValues("InitialAlpha")[x] = 255;
                    Particles.GetFloatValues("InitialBlue")[x] = 255;
                    Particles.GetFloatValues("InitialGreen")[x] = 255;
                    Particles.GetFloatValues("InitialRed")[x] = 255;
                    Particles.GetFloatValues("Altitude")[x] = 0;
                    Particles.GetFloatValues("AltitudeVelocity")[x] = 0;
                    Particles.GetIntValues("AltitudeBounceCount")[x] = 0;
                }
            }

            public void Update(float timeSinceLastFrame)
            {
                var offset = Vector<float>.Count;
                var deltaVector = new Vector<float>(timeSinceLastFrame);
                var timeAlive = Particles.GetFloatValues("TimeAlive");
                var nearestMultiple = NearestMultiple(Program.ParticleCount, offset);

                var x = 0;
                for (x = 0; x < nearestMultiple; x += offset)
                {
                    var slice = timeAlive.Slice(x, offset);
                    var data = new Vector<float>(slice);
                    (data + deltaVector).CopyTo(slice);
                }
                
                // Remaining items
                for (; x < Program.ParticleCount; ++x)
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
                var velocityX = particles.GetFloatValues("VelocityX");
                var velocityY = particles.GetFloatValues("VelocityY");
                var changeVector = new Vector<float>(Emitter.SizeChange);

                int x;
                var offset = Vector<float>.Count;
                var nearestMultiple = NearestMultiple(Program.ParticleCount, offset);
                for (x = 0; x < nearestMultiple; x += offset)
                {
                    var xSlice = velocityX.Slice(x, offset);
                    var ySlice = velocityY.Slice(x, offset);

                    var xVector = new Vector<float>(xSlice);
                    var yVector = new Vector<float>(ySlice);

                    var xDeltaVector = changeVector * timeSinceLastFrame;
                    var yDeltaVector = changeVector * timeSinceLastFrame;

                    (xVector + xDeltaVector).CopyTo(xSlice);
                    (yVector + yDeltaVector).CopyTo(ySlice);
                }
                
                for (; x < Program.ParticleCount; ++x)
                {
                    velocityX[x] += timeSinceLastFrame * Emitter.SizeChange;
                    velocityY[x] += timeSinceLastFrame * Emitter.SizeChange;
                }
            }
        }
        
        public class Modifier3 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                var sizeX = particles.GetFloatValues("SizeX");
                var sizeY = particles.GetFloatValues("SizeY");
                
                var sizeXChangeVector = new Vector<float>(Emitter.SizeChange);
                var sizeYChangeVector = new Vector<float>(Emitter.SizeChange);

                var offset = Vector<float>.Count;
                
                int x;
                var nearestMultiple = NearestMultiple(Program.ParticleCount, offset);
                for (x = 0; x < nearestMultiple; x += offset)
                {
                    var sizeXSlice = sizeX.Slice(x, offset);
                    var sizeYSlice = sizeY.Slice(x, offset);

                    var sizeXVector = new Vector<float>(sizeXSlice);
                    var sizeYVector = new Vector<float>(sizeYSlice);

                    var deltaXVector = sizeXChangeVector * timeSinceLastFrame;
                    var deltaYVector = sizeYChangeVector * timeSinceLastFrame;
                    
                    (sizeXVector + deltaXVector).CopyTo(sizeXSlice);
                    (sizeYVector + deltaYVector).CopyTo(sizeYSlice);
                }
                
                // Remaining items
                for (; x < Program.ParticleCount; ++x)
                {
                    sizeX[x] += timeSinceLastFrame * Emitter.SizeChange;
                    sizeY[x] += timeSinceLastFrame * Emitter.SizeChange;
                }
            }
        }
        
        public class Modifier4 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                var velocityX = particles.GetFloatValues("VelocityX");
                var velocityY = particles.GetFloatValues("VelocityY");

                int x;
                var offset = Vector<float>.Count;
                var nearestMultiple = NearestMultiple(Program.ParticleCount, offset);
                for (x = 0; x < nearestMultiple; x += offset)
                {
                    var xSlice = velocityX.Slice(x, offset);
                    var ySlice = velocityY.Slice(x, offset);

                    var xVector = new Vector<float>(xSlice);
                    var yVector = new Vector<float>(ySlice);

                    var xDeltaVector = (xVector * Emitter.Drag) * timeSinceLastFrame;
                    var yDeltaVector = (yVector * Emitter.Drag) * timeSinceLastFrame;
                    
                    (xVector - xDeltaVector).CopyTo(xSlice);
                    (yVector - yDeltaVector).CopyTo(ySlice);
                }
                
                // Remaining
                for (; x < Program.ParticleCount; x++)
                {
                    velocityX[x] -= Emitter.Drag * velocityX[x] * timeSinceLastFrame;
                    velocityY[x] -= Emitter.Drag * velocityY[x] * timeSinceLastFrame;
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
                var initialRed = particles.GetFloatValues("InitialRed");
                var initialGreen = particles.GetFloatValues("InitialGreen");
                var initialBlue = particles.GetFloatValues("InitialBlue");
                var initialAlpha = particles.GetFloatValues("InitialAlpha");

                var offset = Vector<float>.Count;
                var endValueVector = new Vector<float>(Emitter.EndValue);
                var lifetimeVector = new Vector<float>(Emitter.MaxParticleLifeTime);
                var nearestMultiple = NearestMultiple(Program.ParticleCount, offset);
                
                int x;
                for (x = 0; x < nearestMultiple; x += offset)
                {
                    // Setup vectors
                    var currentRedSlice = currentRed.Slice(x, offset);
                    var currentRedVector = new Vector<float>(currentRedSlice);
                    var currentGreenSlice = currentGreen.Slice(x, offset);
                    var currentGreenVector = new Vector<float>(currentGreenSlice);
                    var currentBlueSlice = currentBlue.Slice(x, offset);
                    var currentBlueVector = new Vector<float>(currentBlueSlice);
                    var currentAlphaSlice = currentAlpha.Slice(x, offset);
                    var currentAlphaVector = new Vector<float>(currentAlphaSlice);

                    var initialRedVector = new Vector<float>(initialRed.Slice(x, offset));
                    var initialGreenVector = new Vector<float>(initialGreen.Slice(x, offset));
                    var initialBlueVector = new Vector<float>(initialBlue.Slice(x, offset));
                    var initialAlphaVector = new Vector<float>(initialAlpha.Slice(x, offset));
                    
                    // Compute interpolated value
                    var interpolatedRedChangeVector = ((initialRedVector - endValueVector) / lifetimeVector) * timeSinceLastFrame;
                    var interpolatedGreenChangeVector = ((initialGreenVector - endValueVector) / lifetimeVector) * timeSinceLastFrame;
                    var interpolatedBlueChangeVector = ((initialBlueVector - endValueVector) / lifetimeVector) * timeSinceLastFrame;
                    var interpolatedAlphaChangeVector = ((initialAlphaVector - endValueVector) / lifetimeVector) * timeSinceLastFrame;
                    
                    // Apply to current values
                    (currentRedVector - interpolatedRedChangeVector).CopyTo(currentRedSlice);
                    (currentGreenVector - interpolatedGreenChangeVector).CopyTo(currentGreenSlice);
                    (currentBlueVector - interpolatedBlueChangeVector).CopyTo(currentBlueSlice);
                    (currentAlphaVector - interpolatedAlphaChangeVector).CopyTo(currentAlphaSlice);
                }
                
                // Remaining values
                for (; x < Program.ParticleCount; ++x)
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
                var sizeX = particles.GetFloatValues("SizeX");
                var sizeY = particles.GetFloatValues("SizeY");
                var initialSizeX = particles.GetFloatValues("InitialSizeX");
                var initialSizeY = particles.GetFloatValues("InitialSizeY");

                var endValueVector = new Vector<float>(Emitter.EndValue);
                var lifetimeVector = new Vector<float>(Emitter.MaxParticleLifeTime);
                
                int x;
                var offset = Vector<float>.Count;
                var nearestMultiple = NearestMultiple(Program.ParticleCount, offset);
                for (x = 0; x < nearestMultiple; x += offset)
                {
                    var sizeXSlice = sizeX.Slice(x, offset);
                    var sizeYSlice = sizeY.Slice(x, offset);

                    var sizeXVector = new Vector<float>(sizeXSlice);
                    var sizeYVector = new Vector<float>(sizeYSlice);
                    var initialXVector = new Vector<float>(initialSizeX.Slice(x, offset));
                    var initialYVector = new Vector<float>(initialSizeY.Slice(x, offset));

                    var deltaXValue = (initialXVector - endValueVector) / lifetimeVector * timeSinceLastFrame;
                    var deltaYValue = (initialYVector - endValueVector) / lifetimeVector * timeSinceLastFrame;
                    
                    (sizeXVector - deltaXValue).CopyTo(sizeXSlice);
                    (sizeYVector - deltaYValue).CopyTo(sizeYSlice);
                }
                
                // Remaining items
                for (; x < Program.ParticleCount; ++x)
                {
                    var width = (((initialSizeX[x] - Emitter.EndValue) / Emitter.MaxParticleLifeTime) *
                                 timeSinceLastFrame);
                    var height = (((initialSizeY[x] - Emitter.EndValue) / Emitter.MaxParticleLifeTime) *
                                  timeSinceLastFrame);
                    sizeX[x] -= width;
                    sizeY[x] -= height;
                }
            }
        }
        
        public class Modifier7 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                var velocityX = particles.GetFloatValues("VelocityX");
                var velocityY = particles.GetFloatValues("VelocityY");
                
                var rotationInRadians = particles.GetFloatValues("RotationInRadians");
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    rotationInRadians[x] = (float) Math.Atan2(velocityY[x], velocityX[x]);
                }
            }
        }
        
        public class Modifier8 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                var velocityX = particles.GetFloatValues("VelocityX");
                var velocityY = particles.GetFloatValues("VelocityY");
                var positionX = particles.GetFloatValues("PositionX");
                var positionY = particles.GetFloatValues("PositionY");
                var refPosX = particles.GetFloatValues("ReferencePositionX");
                var refPosY = particles.GetFloatValues("ReferencePositionY");
                var altitude = particles.GetFloatValues("Altitude");

                int x;
                var offset = Vector<float>.Count;
                var nearestMultiple = NearestMultiple(Program.ParticleCount, offset);
                for (x = 0; x < nearestMultiple; x += offset)
                {
                    var refPosXSlice = refPosX.Slice(x, offset);
                    var refPosYSlice = refPosY.Slice(x, offset);
                    var posXSlice = positionX.Slice(x, offset);
                    var posYSlice = positionY.Slice(x, offset);

                    var refPosXVector = new Vector<float>(refPosXSlice);
                    var refPosYVector = new Vector<float>(refPosYSlice);
                    var altitudeVector = new Vector<float>(altitude.Slice(x, offset));
                    var velocityXVector = new Vector<float>(velocityX.Slice(x, offset));
                    var velocityYVector = new Vector<float>(velocityY.Slice(x, offset));

                    refPosXVector = (velocityXVector * timeSinceLastFrame) + refPosXVector;
                    refPosYVector = (velocityYVector * timeSinceLastFrame) + refPosYVector;
                    
                    refPosXVector.CopyTo(refPosXSlice);
                    refPosYVector.CopyTo(refPosYSlice);
                    
                    refPosXVector.CopyTo(posXSlice);
                    (refPosYVector + altitudeVector).CopyTo(posYSlice);
                }
                
                // Remaining
                for (; x < Program.ParticleCount; ++x)
                {
                    refPosX[x] += velocityX[x] * timeSinceLastFrame;
                    refPosY[x] += velocityY[x] * timeSinceLastFrame;

                    positionX[x] = refPosX[x];
                    positionY[x] = refPosY[x] + altitude[x];
                }
            }
        }

        public class Modifier9 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                var offset = Vector<float>.Count;
                var deltaVector = new Vector<float>(timeSinceLastFrame);
                
                var rotationInRadians = particles.GetFloatValues("RotationInRadians");
                var rotationalVelocityInRadians = particles.GetFloatValues("RotationalVelocityInRadians");

                var nearestMultiple = NearestMultiple(Program.ParticleCount, offset);
                int x;
                for (x = 0; x < nearestMultiple; x += offset)
                {
                    var rotationsSlice = rotationInRadians.Slice(x, offset);
                    var rotationVector = new Vector<float>(rotationsSlice);
                        
                    var velocityVector = new Vector<float>(rotationalVelocityInRadians.Slice(x, offset));
                    var velocitySinceLastFrame = velocityVector * deltaVector;

                    (rotationVector + velocitySinceLastFrame).CopyTo(rotationsSlice);
                }
                
                // Remaining values
                for (; x < Program.ParticleCount; ++x)
                {
                    rotationInRadians[x] += rotationalVelocityInRadians[x] * timeSinceLastFrame;
                }
            }
        }
    }
}