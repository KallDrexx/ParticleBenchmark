using System;
using System.Numerics;

namespace ParticleBenchmark
{
    /// <summary>
    /// Particles made up of arrays of individual properties.  Each modifier is defined in interfaces, each modifier
    /// runs for all particles before the next modifier is run 
    /// </summary>
    public static class ParticleArrayInterfaces
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
            }

            public void Update(float timeSinceLastFrame)
            {
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    Particles.TimeAlive[x] += timeSinceLastFrame;
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
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    particles.TextureSectionIndex[x] = (byte) ((particles.TimeAlive[x] / Emitter.MaxParticleLifeTime) * 10);
                }
            }
        }

        public class Modifier2 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    particles.Velocity[x] += timeSinceLastFrame * new Vector2(Emitter.SizeChange, Emitter.SizeChange);
                }
            }
        }
        
        public class Modifier3 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                for (var x = 0; x < Program.ParticleCount; x++)
                    particles.Size[x] += timeSinceLastFrame * new Vector2(Emitter.SizeChange, Emitter.SizeChange);
            }
        }
        
        public class Modifier4 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    particles.Velocity[x] -= Emitter.Drag * particles.Velocity[x] * timeSinceLastFrame;
                }
            }
        }
        
        public class Modifier5 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    particles.CurrentRed[x] -= (((particles.InitialRed[x] - Emitter.EndValue) / Emitter.MaxParticleLifeTime) *
                                                timeSinceLastFrame);
                    particles.CurrentGreen[x] -= (((particles.InitialGreen[x] - Emitter.EndValue) / Emitter.MaxParticleLifeTime) *
                                                  timeSinceLastFrame);
                    particles.CurrentBlue[x] -= (((particles.InitialBlue[x] - Emitter.EndValue) / Emitter.MaxParticleLifeTime) *
                                                 timeSinceLastFrame);
                    particles.CurrentAlpha[x] -= (((particles.InitialAlpha[x] - Emitter.EndValue) / Emitter.MaxParticleLifeTime) *
                                                  timeSinceLastFrame);
                }
            }
        }
        
        public class Modifier6 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    var width = (((particles.InitialSize[x].X - Emitter.EndValue) / Emitter.MaxParticleLifeTime) *
                                 timeSinceLastFrame);
                    var height = (((particles.InitialSize[x].Y - Emitter.EndValue) / Emitter.MaxParticleLifeTime) *
                                  timeSinceLastFrame);
                    particles.Size[x].X -= width;
                    particles.Size[x].Y -= height;
                }
            }
        }
        
        public class Modifier7 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    if (particles.Velocity[x] != Vector2.Zero)
                    {
                        particles.RotationInRadians[x] = (float) Math.Atan2(particles.Velocity[x].Y, particles.Velocity[x].X);
                    }
                }
            }
        }
        
        public class Modifier8 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    particles.ReferencePosition[x] += particles.Velocity[x] * timeSinceLastFrame;
                    particles.Position[x].X = particles.ReferencePosition[x].X;
                    particles.Position[x].Y = particles.ReferencePosition[x].Y + particles.Altitude[x];
                }
            }
        }

        public class Modifier9 : IModifier
        {
            public void Modify(float timeSinceLastFrame, ParticleCollection particles)
            {
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    particles.RotationInRadians[x] += particles.RotationalVelocityInRadians[x] * timeSinceLastFrame;
                }
            }
        }
    }
}