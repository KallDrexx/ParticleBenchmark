using Shouldly;
using Xunit;

namespace ParticleBenchmark.Tests
{
    public class ValueValidator
    {
        public ValueValidator()
        {
            Program.ParticleCount = 100;
        }
        
        [Fact]
        public void SingleParticleConcrete_Equals_SingleParticleInterface()
        {
            SingleParticleConcrete.Emitter singleParticleConcreteEmitter = new();
            SingleParticleInterfaces.Emitter singeParticleInterfaceEmitter = new(new SingleParticleInterfaces.IModifier[]
            {
                new SingleParticleInterfaces.Modifier1(), new SingleParticleInterfaces.Modifier2(), 
                new SingleParticleInterfaces.Modifier3(), new SingleParticleInterfaces.Modifier4(), 
                new SingleParticleInterfaces.Modifier5(), new SingleParticleInterfaces.Modifier6(), 
                new SingleParticleInterfaces.Modifier7(), new SingleParticleInterfaces.Modifier8(), 
                new SingleParticleInterfaces.Modifier9(),
            });
            
            for (var x = 0; x < 10; x++)
            {
                singleParticleConcreteEmitter.Update(0.16f);
                singeParticleInterfaceEmitter.Update(0.16f);
            }

            for (var x = 0; x < Program.ParticleCount; x++)
            {
                singleParticleConcreteEmitter.Particles[x].Altitude.ShouldBe(singeParticleInterfaceEmitter.Particles[x].Altitude);
                singleParticleConcreteEmitter.Particles[x].Position.ShouldBe(singeParticleInterfaceEmitter.Particles[x].Position);
                singleParticleConcreteEmitter.Particles[x].Size.ShouldBe(singeParticleInterfaceEmitter.Particles[x].Size);
                singleParticleConcreteEmitter.Particles[x].Velocity.ShouldBe(singeParticleInterfaceEmitter.Particles[x].Velocity);
                singleParticleConcreteEmitter.Particles[x].AltitudeVelocity.ShouldBe(singeParticleInterfaceEmitter.Particles[x].AltitudeVelocity);
                singleParticleConcreteEmitter.Particles[x].CurrentAlpha.ShouldBe(singeParticleInterfaceEmitter.Particles[x].CurrentAlpha);
                singleParticleConcreteEmitter.Particles[x].CurrentBlue.ShouldBe(singeParticleInterfaceEmitter.Particles[x].CurrentBlue);
                singleParticleConcreteEmitter.Particles[x].CurrentGreen.ShouldBe(singeParticleInterfaceEmitter.Particles[x].CurrentGreen);
                singleParticleConcreteEmitter.Particles[x].CurrentRed.ShouldBe(singeParticleInterfaceEmitter.Particles[x].CurrentRed);
                singleParticleConcreteEmitter.Particles[x].ReferencePosition.ShouldBe(singeParticleInterfaceEmitter.Particles[x].ReferencePosition);
                singleParticleConcreteEmitter.Particles[x].TimeAlive.ShouldBe(singeParticleInterfaceEmitter.Particles[x].TimeAlive);
                singleParticleConcreteEmitter.Particles[x].AltitudeBounceCount.ShouldBe(singeParticleInterfaceEmitter.Particles[x].AltitudeBounceCount);
                singleParticleConcreteEmitter.Particles[x].RotationInRadians.ShouldBe(singeParticleInterfaceEmitter.Particles[x].RotationInRadians);
                singleParticleConcreteEmitter.Particles[x].TextureSectionIndex.ShouldBe(singeParticleInterfaceEmitter.Particles[x].TextureSectionIndex);
                singleParticleConcreteEmitter.Particles[x].RotationalVelocityInRadians.ShouldBe(singeParticleInterfaceEmitter.Particles[x].RotationalVelocityInRadians); 
            }
        }

        [Fact]
        public void SingleParticleConcrete_Equals_ParticleArrayConcrete()
        {
            SingleParticleConcrete.Emitter singleParticleConcreteEmitter = new();
            ParticleArraysConcrete.Emitter particleArrayConcreteEmitter = new();
            
            for (var x = 0; x < 10; x++)
            {
                singleParticleConcreteEmitter.Update(0.16f);
                particleArrayConcreteEmitter.Update(0.16f);
            }

            for (var x = 0; x < Program.ParticleCount; x++)
            {
                singleParticleConcreteEmitter.Particles[x].Altitude.ShouldBe(particleArrayConcreteEmitter.Particles.Altitude[x]);
                singleParticleConcreteEmitter.Particles[x].Position.ShouldBe(particleArrayConcreteEmitter.Particles.Position[x]);
                singleParticleConcreteEmitter.Particles[x].Size.ShouldBe(particleArrayConcreteEmitter.Particles.Size[x]);
                singleParticleConcreteEmitter.Particles[x].Velocity.ShouldBe(particleArrayConcreteEmitter.Particles.Velocity[x]);
                singleParticleConcreteEmitter.Particles[x].AltitudeVelocity.ShouldBe(particleArrayConcreteEmitter.Particles.AltitudeVelocity[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentAlpha.ShouldBe(particleArrayConcreteEmitter.Particles.CurrentAlpha[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentBlue.ShouldBe(particleArrayConcreteEmitter.Particles.CurrentBlue[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentGreen.ShouldBe(particleArrayConcreteEmitter.Particles.CurrentGreen[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentRed.ShouldBe(particleArrayConcreteEmitter.Particles.CurrentRed[x]);
                singleParticleConcreteEmitter.Particles[x].ReferencePosition.ShouldBe(particleArrayConcreteEmitter.Particles.ReferencePosition[x]);
                singleParticleConcreteEmitter.Particles[x].TimeAlive.ShouldBe(particleArrayConcreteEmitter.Particles.TimeAlive[x]);
                singleParticleConcreteEmitter.Particles[x].AltitudeBounceCount.ShouldBe(particleArrayConcreteEmitter.Particles.AltitudeBounceCount[x]);
                singleParticleConcreteEmitter.Particles[x].RotationInRadians.ShouldBe(particleArrayConcreteEmitter.Particles.RotationInRadians[x]);
                singleParticleConcreteEmitter.Particles[x].TextureSectionIndex.ShouldBe(particleArrayConcreteEmitter.Particles.TextureSectionIndex[x]);
                singleParticleConcreteEmitter.Particles[x].RotationalVelocityInRadians.ShouldBe(particleArrayConcreteEmitter.Particles.RotationalVelocityInRadians[x]); 
            }
        }
        
        [Fact]
        public void SingleParticleConcrete_Equals_ParticleArrayConcreteMultipleIterations()
        {
            SingleParticleConcrete.Emitter singleParticleConcreteEmitter = new();
            ParticleArrayConcreteMultipleIterations.Emitter particleArrayConcreteEmitter = new();
            
            for (var x = 0; x < 10; x++)
            {
                singleParticleConcreteEmitter.Update(0.16f);
                particleArrayConcreteEmitter.Update(0.16f);
            }

            for (var x = 0; x < Program.ParticleCount; x++)
            {
                singleParticleConcreteEmitter.Particles[x].Altitude.ShouldBe(particleArrayConcreteEmitter.Particles.Altitude[x]);
                singleParticleConcreteEmitter.Particles[x].Position.ShouldBe(particleArrayConcreteEmitter.Particles.Position[x]);
                singleParticleConcreteEmitter.Particles[x].Size.ShouldBe(particleArrayConcreteEmitter.Particles.Size[x]);
                singleParticleConcreteEmitter.Particles[x].Velocity.ShouldBe(particleArrayConcreteEmitter.Particles.Velocity[x]);
                singleParticleConcreteEmitter.Particles[x].AltitudeVelocity.ShouldBe(particleArrayConcreteEmitter.Particles.AltitudeVelocity[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentAlpha.ShouldBe(particleArrayConcreteEmitter.Particles.CurrentAlpha[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentBlue.ShouldBe(particleArrayConcreteEmitter.Particles.CurrentBlue[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentGreen.ShouldBe(particleArrayConcreteEmitter.Particles.CurrentGreen[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentRed.ShouldBe(particleArrayConcreteEmitter.Particles.CurrentRed[x]);
                singleParticleConcreteEmitter.Particles[x].ReferencePosition.ShouldBe(particleArrayConcreteEmitter.Particles.ReferencePosition[x]);
                singleParticleConcreteEmitter.Particles[x].TimeAlive.ShouldBe(particleArrayConcreteEmitter.Particles.TimeAlive[x]);
                singleParticleConcreteEmitter.Particles[x].AltitudeBounceCount.ShouldBe(particleArrayConcreteEmitter.Particles.AltitudeBounceCount[x]);
                singleParticleConcreteEmitter.Particles[x].RotationInRadians.ShouldBe(particleArrayConcreteEmitter.Particles.RotationInRadians[x]);
                singleParticleConcreteEmitter.Particles[x].TextureSectionIndex.ShouldBe(particleArrayConcreteEmitter.Particles.TextureSectionIndex[x]);
                singleParticleConcreteEmitter.Particles[x].RotationalVelocityInRadians.ShouldBe(particleArrayConcreteEmitter.Particles.RotationalVelocityInRadians[x]); 
            }
        }
        
        [Fact]
        public void SingleParticleConcrete_Equals_ParticleArrayInterfaces()
        {
            SingleParticleConcrete.Emitter singleParticleConcreteEmitter = new();
            ParticleArrayInterfaces.Emitter particleArrayInterfacesEmitter = new(new ParticleArrayInterfaces.IModifier[]
            {
                new ParticleArrayInterfaces.Modifier1(), new ParticleArrayInterfaces.Modifier2(), 
                new ParticleArrayInterfaces.Modifier3(), new ParticleArrayInterfaces.Modifier4(), 
                new ParticleArrayInterfaces.Modifier5(), new ParticleArrayInterfaces.Modifier6(), 
                new ParticleArrayInterfaces.Modifier7(), new ParticleArrayInterfaces.Modifier8(), 
                new ParticleArrayInterfaces.Modifier9(),
            });
            
            for (var x = 0; x < 10; x++)
            {
                singleParticleConcreteEmitter.Update(0.16f);
                particleArrayInterfacesEmitter.Update(0.16f);
            }

            for (var x = 0; x < Program.ParticleCount; x++)
            {
                singleParticleConcreteEmitter.Particles[x].Altitude.ShouldBe(particleArrayInterfacesEmitter.Particles.Altitude[x]);
                singleParticleConcreteEmitter.Particles[x].Position.ShouldBe(particleArrayInterfacesEmitter.Particles.Position[x]);
                singleParticleConcreteEmitter.Particles[x].Size.ShouldBe(particleArrayInterfacesEmitter.Particles.Size[x]);
                singleParticleConcreteEmitter.Particles[x].Velocity.ShouldBe(particleArrayInterfacesEmitter.Particles.Velocity[x]);
                singleParticleConcreteEmitter.Particles[x].AltitudeVelocity.ShouldBe(particleArrayInterfacesEmitter.Particles.AltitudeVelocity[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentAlpha.ShouldBe(particleArrayInterfacesEmitter.Particles.CurrentAlpha[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentBlue.ShouldBe(particleArrayInterfacesEmitter.Particles.CurrentBlue[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentGreen.ShouldBe(particleArrayInterfacesEmitter.Particles.CurrentGreen[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentRed.ShouldBe(particleArrayInterfacesEmitter.Particles.CurrentRed[x]);
                singleParticleConcreteEmitter.Particles[x].ReferencePosition.ShouldBe(particleArrayInterfacesEmitter.Particles.ReferencePosition[x]);
                singleParticleConcreteEmitter.Particles[x].TimeAlive.ShouldBe(particleArrayInterfacesEmitter.Particles.TimeAlive[x]);
                singleParticleConcreteEmitter.Particles[x].AltitudeBounceCount.ShouldBe(particleArrayInterfacesEmitter.Particles.AltitudeBounceCount[x]);
                singleParticleConcreteEmitter.Particles[x].RotationInRadians.ShouldBe(particleArrayInterfacesEmitter.Particles.RotationInRadians[x]);
                singleParticleConcreteEmitter.Particles[x].TextureSectionIndex.ShouldBe(particleArrayInterfacesEmitter.Particles.TextureSectionIndex[x]);
                singleParticleConcreteEmitter.Particles[x].RotationalVelocityInRadians.ShouldBe(particleArrayInterfacesEmitter.Particles.RotationalVelocityInRadians[x]); 
            }
        }
        
        [Fact]
        public void SingleParticleConcrete_Equals_ParticleArrayInterfacesSpan()
        {
            SingleParticleConcrete.Emitter singleParticleConcreteEmitter = new();
            ParticleArrayInterfacesSpans.Emitter particleArrayInterfacesSpansEmitter = new(new ParticleArrayInterfacesSpans.IModifier[]
            {
                new ParticleArrayInterfacesSpans.Modifier1(), new ParticleArrayInterfacesSpans.Modifier2(), 
                new ParticleArrayInterfacesSpans.Modifier3(), new ParticleArrayInterfacesSpans.Modifier4(), 
                new ParticleArrayInterfacesSpans.Modifier5(), new ParticleArrayInterfacesSpans.Modifier6(), 
                new ParticleArrayInterfacesSpans.Modifier7(), new ParticleArrayInterfacesSpans.Modifier8(), 
                new ParticleArrayInterfacesSpans.Modifier9(),
            });
            
            for (var x = 0; x < 10; x++)
            {
                singleParticleConcreteEmitter.Update(0.16f);
                particleArrayInterfacesSpansEmitter.Update(0.16f);
            }

            for (var x = 0; x < Program.ParticleCount; x++)
            {
                singleParticleConcreteEmitter.Particles[x].Altitude.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.Altitude[x]);
                singleParticleConcreteEmitter.Particles[x].Position.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.Position[x]);
                singleParticleConcreteEmitter.Particles[x].Size.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.Size[x]);
                singleParticleConcreteEmitter.Particles[x].Velocity.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.Velocity[x]);
                singleParticleConcreteEmitter.Particles[x].AltitudeVelocity.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.AltitudeVelocity[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentAlpha.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.CurrentAlpha[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentBlue.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.CurrentBlue[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentGreen.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.CurrentGreen[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentRed.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.CurrentRed[x]);
                singleParticleConcreteEmitter.Particles[x].ReferencePosition.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.ReferencePosition[x]);
                singleParticleConcreteEmitter.Particles[x].TimeAlive.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.TimeAlive[x]);
                singleParticleConcreteEmitter.Particles[x].AltitudeBounceCount.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.AltitudeBounceCount[x]);
                singleParticleConcreteEmitter.Particles[x].RotationInRadians.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.RotationInRadians[x]);
                singleParticleConcreteEmitter.Particles[x].TextureSectionIndex.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.TextureSectionIndex[x]);
                singleParticleConcreteEmitter.Particles[x].RotationalVelocityInRadians.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.RotationalVelocityInRadians[x]); 
            }
        }
        
        [Fact]
        public void SingleParticleConcrete_Equals_ParticleArrayInterfacesSpan2()
        {
            SingleParticleConcrete.Emitter singleParticleConcreteEmitter = new();
            ParticleArrayInterfacesSpans2.Emitter particleArrayInterfacesSpansEmitter = new(new ParticleArrayInterfacesSpans2.IModifier[]
            {
                new ParticleArrayInterfacesSpans2.Modifier1(), new ParticleArrayInterfacesSpans2.Modifier2(), 
                new ParticleArrayInterfacesSpans2.Modifier3(), new ParticleArrayInterfacesSpans2.Modifier4(), 
                new ParticleArrayInterfacesSpans2.Modifier5(), new ParticleArrayInterfacesSpans2.Modifier6(), 
                new ParticleArrayInterfacesSpans2.Modifier7(), new ParticleArrayInterfacesSpans2.Modifier8(), 
                new ParticleArrayInterfacesSpans2.Modifier9(),
            });
            
            for (var x = 0; x < 10; x++)
            {
                singleParticleConcreteEmitter.Update(0.16f);
                particleArrayInterfacesSpansEmitter.Update(0.16f);
            }

            for (var x = 0; x < Program.ParticleCount; x++)
            {
                singleParticleConcreteEmitter.Particles[x].Altitude.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.GetFloatValues("Altitude")[x]);
                singleParticleConcreteEmitter.Particles[x].Position.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.GetVector2Values("Position")[x]);
                singleParticleConcreteEmitter.Particles[x].Size.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.GetVector2Values("Size")[x]);
                singleParticleConcreteEmitter.Particles[x].Velocity.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.GetVector2Values("Velocity")[x]);
                singleParticleConcreteEmitter.Particles[x].AltitudeVelocity.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.GetFloatValues("AltitudeVelocity")[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentAlpha.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.GetFloatValues("CurrentAlpha")[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentBlue.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.GetFloatValues("CurrentBlue")[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentGreen.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.GetFloatValues("CurrentGreen")[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentRed.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.GetFloatValues("CurrentRed")[x]);
                singleParticleConcreteEmitter.Particles[x].ReferencePosition.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.GetVector2Values("ReferencePosition")[x]);
                singleParticleConcreteEmitter.Particles[x].TimeAlive.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.GetFloatValues("TimeAlive")[x]);
                singleParticleConcreteEmitter.Particles[x].AltitudeBounceCount.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.GetIntValues("AltitudeBounceCount")[x]);
                singleParticleConcreteEmitter.Particles[x].RotationInRadians.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.GetFloatValues("RotationInRadians")[x]);
                singleParticleConcreteEmitter.Particles[x].TextureSectionIndex.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.GetByteValues("TextureSectionIndex")[x]);
                singleParticleConcreteEmitter.Particles[x].RotationalVelocityInRadians.ShouldBe(particleArrayInterfacesSpansEmitter.Particles.GetFloatValues("RotationalVelocityInRadians")[x]); 
            }
        }

        [Fact]
        public void SingleParticleConcrete_Equals_ParticleArrayInterfacesSimd()
        {
            SingleParticleConcrete.Emitter singleParticleConcreteEmitter = new();
            ParticleArrayInterfacesSimd.Emitter particleArrayInterfacesSimdEmitter = new(new ParticleArrayInterfacesSimd.IModifier[]
            {
                new ParticleArrayInterfacesSimd.Modifier1(), new ParticleArrayInterfacesSimd.Modifier2(), 
                new ParticleArrayInterfacesSimd.Modifier3(), new ParticleArrayInterfacesSimd.Modifier4(), 
                new ParticleArrayInterfacesSimd.Modifier5(), new ParticleArrayInterfacesSimd.Modifier6(), 
                new ParticleArrayInterfacesSimd.Modifier7(), new ParticleArrayInterfacesSimd.Modifier8(), 
                new ParticleArrayInterfacesSimd.Modifier9(),
            });
            
            for (var x = 0; x < 10; x++)
            {
                singleParticleConcreteEmitter.Update(0.16f);
                particleArrayInterfacesSimdEmitter.Update(0.16f);
            }

            for (var x = 0; x < Program.ParticleCount; x++)
            {
                singleParticleConcreteEmitter.Particles[x].TimeAlive.ShouldBe(particleArrayInterfacesSimdEmitter.Particles.GetFloatValues("TimeAlive")[x]);
                singleParticleConcreteEmitter.Particles[x].Altitude.ShouldBe(particleArrayInterfacesSimdEmitter.Particles.GetFloatValues("Altitude")[x]);
                singleParticleConcreteEmitter.Particles[x].Position.X.ShouldBe(particleArrayInterfacesSimdEmitter.Particles.GetFloatValues("PositionX")[x]);
                singleParticleConcreteEmitter.Particles[x].Position.Y.ShouldBe(particleArrayInterfacesSimdEmitter.Particles.GetFloatValues("PositionY")[x]);
                singleParticleConcreteEmitter.Particles[x].Size.X.ShouldBe(particleArrayInterfacesSimdEmitter.Particles.GetFloatValues("SizeX")[x]);
                singleParticleConcreteEmitter.Particles[x].Size.Y.ShouldBe(particleArrayInterfacesSimdEmitter.Particles.GetFloatValues("SizeY")[x]);
                singleParticleConcreteEmitter.Particles[x].Velocity.X.ShouldBe(particleArrayInterfacesSimdEmitter.Particles.GetFloatValues("VelocityX")[x]);
                singleParticleConcreteEmitter.Particles[x].Velocity.Y.ShouldBe(particleArrayInterfacesSimdEmitter.Particles.GetFloatValues("VelocityY")[x]);
                singleParticleConcreteEmitter.Particles[x].AltitudeVelocity.ShouldBe(particleArrayInterfacesSimdEmitter.Particles.GetFloatValues("AltitudeVelocity")[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentAlpha.ShouldBe(particleArrayInterfacesSimdEmitter.Particles.GetFloatValues("CurrentAlpha")[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentBlue.ShouldBe(particleArrayInterfacesSimdEmitter.Particles.GetFloatValues("CurrentBlue")[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentGreen.ShouldBe(particleArrayInterfacesSimdEmitter.Particles.GetFloatValues("CurrentGreen")[x]);
                singleParticleConcreteEmitter.Particles[x].CurrentRed.ShouldBe(particleArrayInterfacesSimdEmitter.Particles.GetFloatValues("CurrentRed")[x]);
                singleParticleConcreteEmitter.Particles[x].ReferencePosition.X.ShouldBe(particleArrayInterfacesSimdEmitter.Particles.GetFloatValues("ReferencePositionX")[x]);
                singleParticleConcreteEmitter.Particles[x].ReferencePosition.Y.ShouldBe(particleArrayInterfacesSimdEmitter.Particles.GetFloatValues("ReferencePositionY")[x]);
                singleParticleConcreteEmitter.Particles[x].TimeAlive.ShouldBe(particleArrayInterfacesSimdEmitter.Particles.GetFloatValues("TimeAlive")[x]);
                singleParticleConcreteEmitter.Particles[x].AltitudeBounceCount.ShouldBe(particleArrayInterfacesSimdEmitter.Particles.GetIntValues("AltitudeBounceCount")[x]);
                singleParticleConcreteEmitter.Particles[x].RotationInRadians.ShouldBe(particleArrayInterfacesSimdEmitter.Particles.GetFloatValues("RotationInRadians")[x]);
                singleParticleConcreteEmitter.Particles[x].TextureSectionIndex.ShouldBe(particleArrayInterfacesSimdEmitter.Particles.GetByteValues("TextureSectionIndex")[x]);
                singleParticleConcreteEmitter.Particles[x].RotationalVelocityInRadians.ShouldBe(particleArrayInterfacesSimdEmitter.Particles.GetFloatValues("RotationalVelocityInRadians")[x]); 
            }
        }

        [Fact]
        public void SingleParticleConcrete_Equals_ComputeShaderParticles()
        {
            SingleParticleConcrete.Emitter singleParticleConcreteEmitter = new();
            ComputeShaderParticles.Emitter particleArrayInterfacesSimdEmitter = new();
            
            for (var x = 0; x < 10; x++)
            {
                singleParticleConcreteEmitter.Update(0.16f);
                particleArrayInterfacesSimdEmitter.Update(0.16f);
            }

            for (var x = 0; x < Program.ParticleCount; x++)
            {
                singleParticleConcreteEmitter.Particles[x].TimeAlive.ShouldBe(particleArrayInterfacesSimdEmitter.Particles[x].TimeAlive);
                singleParticleConcreteEmitter.Particles[x].Altitude.ShouldBe(particleArrayInterfacesSimdEmitter.Particles[x].Altitude);
                singleParticleConcreteEmitter.Particles[x].Position.X.ShouldBe(particleArrayInterfacesSimdEmitter.Particles[x].PositionX);
                singleParticleConcreteEmitter.Particles[x].Position.Y.ShouldBe(particleArrayInterfacesSimdEmitter.Particles[x].PositionY);
                singleParticleConcreteEmitter.Particles[x].Size.X.ShouldBe(particleArrayInterfacesSimdEmitter.Particles[x].SizeX);
                singleParticleConcreteEmitter.Particles[x].Size.Y.ShouldBe(particleArrayInterfacesSimdEmitter.Particles[x].SizeY);
                singleParticleConcreteEmitter.Particles[x].Velocity.X.ShouldBe(particleArrayInterfacesSimdEmitter.Particles[x].VelocityX);
                singleParticleConcreteEmitter.Particles[x].Velocity.Y.ShouldBe(particleArrayInterfacesSimdEmitter.Particles[x].VelocityY);
                singleParticleConcreteEmitter.Particles[x].AltitudeVelocity.ShouldBe(particleArrayInterfacesSimdEmitter.Particles[x].AltitudeVelocity);
                singleParticleConcreteEmitter.Particles[x].CurrentAlpha.ShouldBe(particleArrayInterfacesSimdEmitter.Particles[x].CurrentAlpha);
                singleParticleConcreteEmitter.Particles[x].CurrentBlue.ShouldBe(particleArrayInterfacesSimdEmitter.Particles[x].CurrentBlue);
                singleParticleConcreteEmitter.Particles[x].CurrentGreen.ShouldBe(particleArrayInterfacesSimdEmitter.Particles[x].CurrentGreen);
                singleParticleConcreteEmitter.Particles[x].CurrentRed.ShouldBe(particleArrayInterfacesSimdEmitter.Particles[x].CurrentRed);
                singleParticleConcreteEmitter.Particles[x].ReferencePosition.X.ShouldBe(particleArrayInterfacesSimdEmitter.Particles[x].ReferencePositionX);
                singleParticleConcreteEmitter.Particles[x].ReferencePosition.Y.ShouldBe(particleArrayInterfacesSimdEmitter.Particles[x].ReferencePositionY);
                singleParticleConcreteEmitter.Particles[x].TimeAlive.ShouldBe(particleArrayInterfacesSimdEmitter.Particles[x].TimeAlive);
                singleParticleConcreteEmitter.Particles[x].RotationInRadians.ShouldBe(particleArrayInterfacesSimdEmitter.Particles[x].RotationInRadians);
                ((int)singleParticleConcreteEmitter.Particles[x].TextureSectionIndex).ShouldBe(particleArrayInterfacesSimdEmitter.Particles[x].TextureSectionIndex);
                singleParticleConcreteEmitter.Particles[x].RotationalVelocityInRadians.ShouldBe(particleArrayInterfacesSimdEmitter.Particles[x].RotationalVelocityInRadians); 
            }
        }
    }
}