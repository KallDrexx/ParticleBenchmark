using System;
using System.Numerics;

namespace ParticleBenchmark
{
    /// <summary>
    /// Single holistic particle struct, each modification is hard coded.  Each modification is run on all particles
    /// before moving onto the next modification section.
    /// </summary>
    public static class SingleParticleConcreteMultipleIteration
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

            public Emitter()
            {
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

                // modifiers
                for (var x = 0; x < _particles.Length; x++)
                {
                    {
                        _particles[x].TextureSectionIndex = (byte) ((_particles[x].TimeAlive / 1000) * 10);
                    }
                }

                for (var x = 0; x < _particles.Length; x++)
                {
                    {
                        _particles[x].Velocity += timeSinceLastFrame * new Vector2(5, 5);
                    }
                }

                for (var x = 0; x < _particles.Length; x++)
                {
                    {
                        _particles[x].Size += timeSinceLastFrame * new Vector2(5, 5);
                    }
                }

                for (var x = 0; x < _particles.Length; x++)
                {
                    {
                        _particles[x].Velocity -= 0.1f * _particles[x].Velocity * timeSinceLastFrame;
                    }
                }

                for (var x = 0; x < _particles.Length; x++)
                {
                    {
                        _particles[x].CurrentRed -= (((_particles[x].InitialRed - 0) / 1000f) *
                                                timeSinceLastFrame);
                        _particles[x].CurrentGreen -= (((_particles[x].InitialGreen - 0) / 1000f) *
                                                  timeSinceLastFrame);
                        _particles[x].CurrentBlue -= (((_particles[x].InitialBlue - 0) / 1000f) *
                                                 timeSinceLastFrame);
                        _particles[x].CurrentAlpha -= (((_particles[x].InitialAlpha - 0) / 1000f) *
                                                  timeSinceLastFrame);
                    }
                }

                for (var x = 0; x < _particles.Length; x++)
                {
                    {

                        var width = (((_particles[x].InitialSize.X - 0) / 1000f) *
                                     timeSinceLastFrame);
                        var height = (((_particles[x].InitialSize.Y - 0) / 1000f) *
                                      timeSinceLastFrame);
                        _particles[x].Size.X -= width;
                        _particles[x].Size.Y -= height;
                    }
                }

                for (var x = 0; x < _particles.Length; x++)
                {
                    {
                        if (_particles[x].Velocity != Vector2.Zero)
                        {
                            _particles[x].RotationInRadians = (float) Math.Atan2(_particles[x].Velocity.Y, _particles[x].Velocity.X);
                        }
                    }
                }

                // position modifier

                for (var x = 0; x < _particles.Length; x++)
                {
                    _particles[x].ReferencePosition += _particles[x].Velocity * timeSinceLastFrame;
                    _particles[x].Position.X = _particles[x].ReferencePosition.X;
                    _particles[x].Position.Y = _particles[x].ReferencePosition.Y + _particles[x].Altitude;
                }

                for (var x = 0; x < _particles.Length; x++)
                {
                    _particles[x].RotationInRadians += _particles[x].RotationalVelocityInRadians * timeSinceLastFrame;
                }
            }
        }
    }
}