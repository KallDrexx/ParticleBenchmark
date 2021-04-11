using Shouldly;
using Xunit;

namespace ParticleBenchmark.Tests
{
    public class ValueValidator
    {
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
    }
}