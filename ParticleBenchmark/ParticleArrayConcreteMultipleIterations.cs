using System;
using System.Numerics;

namespace ParticleBenchmark
{
    /// <summary>
    /// Particles made up of arrays of individual properties.  Each modifier is hard coded, each modifier runs for
    /// all particles before the next modifier is run
    /// </summary>
    public static class ParticleArrayConcreteMultipleIterations
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
            public float MaxParticleLifeTime { get; set; } = 5f;
            public float SizeChange { get; set; } = 5f;
            public float EndValue { get; set; } = 0f;
            public float Drag { get; set; } = 0.1f;

            public readonly ParticleCollection Particles = new ParticleCollection();

            public Emitter()
            {
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

                // modifiers
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    Particles.TextureSectionIndex[x] = (byte) ((Particles.TimeAlive[x] / MaxParticleLifeTime) * 10);
                }
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    Particles.Velocity[x] += timeSinceLastFrame * new Vector2(SizeChange, SizeChange);
                }
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    Particles.Size[x] += timeSinceLastFrame * new Vector2(SizeChange, SizeChange);
                }
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    Particles.Velocity[x] -= Drag * Particles.Velocity[x] * timeSinceLastFrame;
                }
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    Particles.CurrentRed[x] -= (((Particles.InitialRed[x] - EndValue) / MaxParticleLifeTime) *
                                            timeSinceLastFrame);
                    Particles.CurrentGreen[x] -= (((Particles.InitialGreen[x] - EndValue) / MaxParticleLifeTime) *
                                              timeSinceLastFrame);
                    Particles.CurrentBlue[x] -= (((Particles.InitialBlue[x] - EndValue) / MaxParticleLifeTime) *
                                             timeSinceLastFrame);
                    Particles.CurrentAlpha[x] -= (((Particles.InitialAlpha[x] - EndValue) / MaxParticleLifeTime) *
                                              timeSinceLastFrame);
                }
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {

                    var width = (((Particles.InitialSize[x].X - EndValue) / MaxParticleLifeTime) *
                                 timeSinceLastFrame);
                    var height = (((Particles.InitialSize[x].Y - EndValue) / MaxParticleLifeTime) *
                                  timeSinceLastFrame);
                    Particles.Size[x].X -= width;
                    Particles.Size[x].Y -= height;
                }
                
                for (var x = 0; x < Program.ParticleCount; x++)
                {

                    if (Particles.Velocity[x] != Vector2.Zero)
                    {
                        Particles.RotationInRadians[x] = (float) Math.Atan2(Particles.Velocity[x].Y, Particles.Velocity[x].X);
                    }
                }

                    // position modifier

                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    Particles.ReferencePosition[x] += Particles.Velocity[x] * timeSinceLastFrame;
                    Particles.Position[x].X = Particles.ReferencePosition[x].X;
                    Particles.Position[x].Y = Particles.ReferencePosition[x].Y + Particles.Altitude[x];
                }

                for (var x = 0; x < Program.ParticleCount; x++)
                {
                    Particles.RotationInRadians[x] += Particles.RotationalVelocityInRadians[x] * timeSinceLastFrame;
                }
            }
        }
    }
}