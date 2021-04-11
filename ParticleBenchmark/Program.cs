using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace ParticleBenchmark
{
    public class Program
    {
        public static int ParticleCount = 1000;
        
        [SimpleJob(RuntimeMoniker.NetCoreApp50, baseline: true)]
        public class Benchmark
        {
            private readonly SingleParticleConcrete.Emitter _singleParticleConcreteEmitter = new();
            private readonly SingleParticleInterfaces.Emitter _singeParticleInterfaceEmitter = new(new SingleParticleInterfaces.IModifier[]
            {
                new SingleParticleInterfaces.Modifier1(), new SingleParticleInterfaces.Modifier2(), 
                new SingleParticleInterfaces.Modifier3(), new SingleParticleInterfaces.Modifier4(), 
                new SingleParticleInterfaces.Modifier5(), new SingleParticleInterfaces.Modifier6(), 
                new SingleParticleInterfaces.Modifier7(), new SingleParticleInterfaces.Modifier8(), 
                new SingleParticleInterfaces.Modifier9(),
            });
            
            private readonly SingleParticleConcreteMultipleIteration.Emitter _singleMultipleIteration = new();
            private readonly ParticleArraysConcrete.Emitter _particleArrayConcreteEmitter = new();
            private readonly ParticleArrayConcreteMultipleIterations.Emitter _particleArrayConcreteIterationsEmitter = new();

            private readonly ParticleArrayInterfaces.Emitter _particleArrayInterfacesEmitter =
                new(new ParticleArrayInterfaces.IModifier[]
                {
                    new ParticleArrayInterfaces.Modifier1(), new ParticleArrayInterfaces.Modifier2(),
                    new ParticleArrayInterfaces.Modifier3(), new ParticleArrayInterfaces.Modifier4(),
                    new ParticleArrayInterfaces.Modifier5(), new ParticleArrayInterfaces.Modifier6(),
                    new ParticleArrayInterfaces.Modifier7(), new ParticleArrayInterfaces.Modifier8(),
                    new ParticleArrayInterfaces.Modifier9(),
                });

            [Benchmark(Baseline = true)]
            public float SingleParticleConcrete()
            {
                _singleParticleConcreteEmitter.Update(0.16f);
                return _singleParticleConcreteEmitter.Particles[0].Velocity.X;
            }

            [Benchmark]
            public float SingleParticleInterfaces()
            {
                _singeParticleInterfaceEmitter.Update(0.16f);
                return _singeParticleInterfaceEmitter.Particles[0].Velocity.X;
            }

            [Benchmark]
            public float SingleParticleMultipleIteration()
            {
                _singleMultipleIteration.Update(0.16f);
                return _singleMultipleIteration._particles[0].Velocity.X;
            }

            [Benchmark]
            public float ParticleArrayConcrete()
            {
                _particleArrayConcreteEmitter.Update(0.16f);
                return _particleArrayConcreteEmitter.Particles.Velocity[0].X;
            }

            [Benchmark]
            public float ParticleArrayConcreteIterations()
            {
                _particleArrayConcreteIterationsEmitter.Update(0.16f);
                return _particleArrayConcreteIterationsEmitter.Particles.Velocity[0].X;
            }

            [Benchmark]
            public float ParticleArrayInterfaces()
            {
                _particleArrayInterfacesEmitter.Update(0.16f);
                return _particleArrayInterfacesEmitter.Particles.Velocity[0].X;
            }
        }
        
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Program.Benchmark>();
        }
    }
}