using System;
using System.Numerics;

namespace ParticleBenchmark
{
    public static class SingleParticleInterfaces
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
            public readonly Particle[] _particles = new Particle[Program.ParticleCount];

            private readonly IModifier[] _modifiers;

            public Emitter(int count)
            {
                if (count == 9)
                {
                    _modifiers = new IModifier[]
                    {
                        new Modifier1(), new Modifier2(), new Modifier3(), new Modifier4(), new Modifier5(),
                        new Modifier6(), new Modifier7(), new Modifier8(), new Modifier9(),
                    };
                }
                else
                {
                    _modifiers = new IModifier[]
                    {
                        new Modifier1(), new Modifier2(), new Modifier3(), new Modifier4(), new Modifier5(),
                        new Modifier6(), new Modifier7(), new Modifier8(),
                    };
                }
                
                for (var x = 0; x < _particles.Length; x++)
                {
                    _particles[x] = new Particle
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
                for (var x = 0; x < _particles.Length; x++)
                {
                    _particles[x].TimeAlive += timeSinceLastFrame;
                }
                
                foreach (var modifier in _modifiers)
                {
                    modifier.Modify(timeSinceLastFrame, _particles);
                }
            }
        }

        public interface IModifier
        {
            void Modify(float timeSinceLastFrame, Particle[] particles);
        }

        public class Modifier1 : IModifier
        {
            public void Modify(float timeSinceLastFrame, Particle[] particles)
            {
                for (var x = 0; x < particles.Length; x++)
                {
                    particles[x].TextureSectionIndex = (byte) ((particles[x].TimeAlive / 1000) * 10);
                }
            }
        }

        public class Modifier2 : IModifier
        {
            public void Modify(float timeSinceLastFrame, Particle[] particles)
            {
                for (var x = 0; x < particles.Length; x++)
                {
                    particles[x].Velocity += timeSinceLastFrame * new Vector2(5, 5);
                }
            }
        }
        
        public class Modifier3 : IModifier
        {
            public void Modify(float timeSinceLastFrame, Particle[] particles)
            {
                for (var x = 0; x < particles.Length; x++)
                    particles[x].Size += timeSinceLastFrame * new Vector2(5, 5);
            }
        }
        
        public class Modifier4 : IModifier
        {
            public void Modify(float timeSinceLastFrame, Particle[] particles)
            {
                for (var x = 0; x < particles.Length; x++)
                {
                    particles[x].Velocity -= 0.1f * particles[x].Velocity * timeSinceLastFrame;
                }
            }
        }
        
        public class Modifier5 : IModifier
        {
            public void Modify(float timeSinceLastFrame, Particle[] particles)
            {
                for (var x = 0; x < particles.Length; x++)
                {
                    particles[x].CurrentRed -= (((particles[x].InitialRed - 0) / 1000f) *
                                                timeSinceLastFrame);
                    particles[x].CurrentGreen -= (((particles[x].InitialGreen - 0) / 1000f) *
                                                  timeSinceLastFrame);
                    particles[x].CurrentBlue -= (((particles[x].InitialBlue - 0) / 1000f) *
                                                 timeSinceLastFrame);
                    particles[x].CurrentAlpha -= (((particles[x].InitialAlpha - 0) / 1000f) *
                                                  timeSinceLastFrame);
                }
            }
        }
        
        public class Modifier6 : IModifier
        {
            public void Modify(float timeSinceLastFrame, Particle[] particles)
            {
                for (var x = 0; x < particles.Length; x++)
                {
                    var width = (((particles[x].InitialSize.X - 0) / 1000f) *
                                 timeSinceLastFrame);
                    var height = (((particles[x].InitialSize.Y - 0) / 1000f) *
                                  timeSinceLastFrame);
                    particles[x].Size.X -= width;
                    particles[x].Size.Y -= height;
                }
            }
        }
        
        public class Modifier7 : IModifier
        {
            public void Modify(float timeSinceLastFrame, Particle[] particles)
            {
                for (var x = 0; x < particles.Length; x++)
                {
                    if (particles[x].Velocity != Vector2.Zero)
                    {
                        particles[x].RotationInRadians = (float) Math.Atan2(particles[x].Velocity.Y, particles[x].Velocity.X);
                    }
                }
            }
        }
        
        public class Modifier8 : IModifier
        {
            public void Modify(float timeSinceLastFrame, Particle[] particles)
            {
                for (var x = 0; x < particles.Length; x++)
                {
                    particles[x].ReferencePosition += particles[x].Velocity * timeSinceLastFrame;
                    particles[x].Position.X = particles[x].ReferencePosition.X;
                    particles[x].Position.Y = particles[x].ReferencePosition.Y + particles[x].Altitude;
                }
            }
        }

        public class Modifier9 : IModifier
        {
            public void Modify(float timeSinceLastFrame, Particle[] particles)
            {
                for (var x = 0; x < particles.Length; x++)
                {
                    particles[x].RotationInRadians += particles[x].RotationalVelocityInRadians * timeSinceLastFrame;
                }
            }
        }
    }
}