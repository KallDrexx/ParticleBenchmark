using System;
using System.Numerics;

namespace ParticleBenchmark
{
    /// <summary>
    /// Single holistic particle struct.  All modification logic is hard coded in, and each particle
    /// has all it's modifications run before moving onto the next particle
    /// </summary>
    public static class SingleParticleConcrete
    {
        public struct Particle
        {
            public bool IsAlive;
            public byte TextureSectionIndex;
    
            /// <summary>
            /// The standard (non-zoomed) width and height of the particle, in pixels
            /// </summary>
            public Vector2 Size;

            public Vector2 InitialSize;
    
            /// <summary>
            /// The center position of the particle in world space
            /// </summary>
            public Vector2 Position;

            /// <summary>
            /// An optional position that can be used for modifiers that need to modify a position based on some other
            /// reference point (i.e. altitude bouncing)
            /// </summary>
            public Vector2 ReferencePosition;
    
            public Vector2 Velocity;
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
            public float MaxParticleLifeTime { get; set; } = 5f;
            public float SizeChange { get; set; } = 5f;
            public float EndValue { get; set; } = 0f;
            public float Drag { get; set; } = 0.1f;
            
            public readonly Particle[] Particles = new Particle[Program.ParticleCount];

            public Emitter()
            {
                for (var x = 0; x < Particles.Length; x++)
                {
                    Particles[x] = new Particle
                    {
                        IsAlive = true,
                        TimeAlive = 0,
                        RotationInRadians = 1,
                        Position = new Vector2(100, 100),
                        ReferencePosition = new Vector2(100, 100),
                        RotationalVelocityInRadians = 1f,
                        CurrentRed = 255,
                        CurrentGreen = 255,
                        CurrentBlue = 255,
                        CurrentAlpha = 255,
                        Size = Vector2.Zero,
                        InitialSize = new Vector2(32, 32),
                        Velocity = new Vector2(100, 100),
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
            }

            public void Update(float timeSinceLastFrame)
            {
                for (var x = 0; x < Particles.Length; x++)
                {
                    Particles[x].TimeAlive += timeSinceLastFrame;

                    // modifiers
                    {
                        Particles[x].TextureSectionIndex = (byte) ((Particles[x].TimeAlive / MaxParticleLifeTime) * 10);
                    }
                    {
                        Particles[x].Velocity += timeSinceLastFrame * new Vector2(SizeChange, SizeChange);
                    }
                    {
                        Particles[x].Size += timeSinceLastFrame * new Vector2(SizeChange, SizeChange);
                    }
                    {
                        Particles[x].Velocity -= Drag * Particles[x].Velocity * timeSinceLastFrame;
                    }
                    {
                        Particles[x].CurrentRed -= (((Particles[x].InitialRed - EndValue) / MaxParticleLifeTime) *
                                                timeSinceLastFrame);
                        Particles[x].CurrentGreen -= (((Particles[x].InitialGreen - EndValue) / MaxParticleLifeTime) *
                                                  timeSinceLastFrame);
                        Particles[x].CurrentBlue -= (((Particles[x].InitialBlue - EndValue) / MaxParticleLifeTime) *
                                                 timeSinceLastFrame);
                        Particles[x].CurrentAlpha -= (((Particles[x].InitialAlpha - EndValue) / MaxParticleLifeTime) *
                                                  timeSinceLastFrame);
                    }
                    {

                        var width = (((Particles[x].InitialSize.X - EndValue) / MaxParticleLifeTime) *
                                     timeSinceLastFrame);
                        var height = (((Particles[x].InitialSize.Y - EndValue) / MaxParticleLifeTime) *
                                      timeSinceLastFrame);
                        Particles[x].Size.X -= width;
                        Particles[x].Size.Y -= height;
                    }
                    {

                        if (Particles[x].Velocity != Vector2.Zero)
                        {
                            Particles[x].RotationInRadians = (float) Math.Atan2(Particles[x].Velocity.Y, Particles[x].Velocity.X);
                        }
                    }

                    // position modifier

                    Particles[x].ReferencePosition += Particles[x].Velocity * timeSinceLastFrame;
                    Particles[x].Position.X = Particles[x].ReferencePosition.X;
                    Particles[x].Position.Y = Particles[x].ReferencePosition.Y + Particles[x].Altitude;

                    Particles[x].RotationInRadians += Particles[x].RotationalVelocityInRadians * timeSinceLastFrame;
                }
            }
        }
    }
}