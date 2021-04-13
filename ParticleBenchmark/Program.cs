using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace ParticleBenchmark
{
    public class Program
    {
        public static int ParticleCount = 1000000;
        
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
            
            private readonly ParticleArrayInterfacesSpans.Emitter _particleArrayInterfacesSpansEmitter =
                new(new ParticleArrayInterfacesSpans.IModifier[]
                {
                    new ParticleArrayInterfacesSpans.Modifier1(), new ParticleArrayInterfacesSpans.Modifier2(),
                    new ParticleArrayInterfacesSpans.Modifier3(), new ParticleArrayInterfacesSpans.Modifier4(),
                    new ParticleArrayInterfacesSpans.Modifier5(), new ParticleArrayInterfacesSpans.Modifier6(),
                    new ParticleArrayInterfacesSpans.Modifier7(), new ParticleArrayInterfacesSpans.Modifier8(),
                    new ParticleArrayInterfacesSpans.Modifier9(),
                });
            
            private readonly ParticleArrayInterfacesSpans2.Emitter _particleArrayInterfacesSpans2Emitter =
                new(new ParticleArrayInterfacesSpans2.IModifier[]
                {
                    new ParticleArrayInterfacesSpans2.Modifier1(), new ParticleArrayInterfacesSpans2.Modifier2(),
                    new ParticleArrayInterfacesSpans2.Modifier3(), new ParticleArrayInterfacesSpans2.Modifier4(),
                    new ParticleArrayInterfacesSpans2.Modifier5(), new ParticleArrayInterfacesSpans2.Modifier6(),
                    new ParticleArrayInterfacesSpans2.Modifier7(), new ParticleArrayInterfacesSpans2.Modifier8(),
                    new ParticleArrayInterfacesSpans2.Modifier9(),
                });
            
            private readonly ParticleArrayInterfacesSimd.Emitter _particleArrayInterfacesSimdEmitter =
                new(new ParticleArrayInterfacesSimd.IModifier[]
                {
                    new ParticleArrayInterfacesSimd.Modifier1(), new ParticleArrayInterfacesSimd.Modifier2(),
                    new ParticleArrayInterfacesSimd.Modifier3(), new ParticleArrayInterfacesSimd.Modifier4(),
                    new ParticleArrayInterfacesSimd.Modifier5(), new ParticleArrayInterfacesSimd.Modifier6(),
                    new ParticleArrayInterfacesSimd.Modifier7(), new ParticleArrayInterfacesSimd.Modifier8(),
                    new ParticleArrayInterfacesSimd.Modifier9(),
                });

            private readonly ComputeShaderParticles.Emitter _computeShaderEmitter = new();

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

            [Benchmark]
            public float ParticleArrayInterfacesWithSpans()
            {
                _particleArrayInterfacesSpansEmitter.Update(0.16f);
                return _particleArrayInterfacesSpansEmitter.Particles.Velocity[0].X;
            }

            [Benchmark]
            public float ParticleArrayInterfacesWithSpans2()
            {
                _particleArrayInterfacesSpans2Emitter.Update(0.16f);
                return _particleArrayInterfacesSpans2Emitter.Particles.GetVector2Values("Velocity")[0].X;
            }

            [Benchmark]
            public float ParticleArrayInterfacesWithSimd()
            {
                _particleArrayInterfacesSimdEmitter.Update(0.16f);
                return _particleArrayInterfacesSimdEmitter.Particles.GetFloatValues("VelocityX")[0];
            }
            
            [Benchmark]
            public float ComputeShader()
            {
                _computeShaderEmitter.Update(0.16f);
                return _computeShaderEmitter.ParticleProperties["VelocityX"][0];
            }
        }
        
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Program.Benchmark>();
        }
    }
}