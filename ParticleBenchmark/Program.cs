using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace ParticleBenchmark
{
    public class Program
    {
        public static int ParticleCount = 1000;

        public class Benchmark
        {
            private readonly SingleParticleConcrete.Emitter _singleParticleConcreteEmitter = new();
            private readonly SingleParticleInterfaces.Emitter _singeParticleInterfaceEmitter = new(9);
            private readonly SingleParticleConcreteMultipleIteration.Emitter _singleMultipleIteration = new();
            private readonly SingleParticleInterfacesSingle.Emitter _singleInterfaceSingleEmitter = new(9);

            [Benchmark]
            public float SingleParticleConcrete()
            {
                _singleParticleConcreteEmitter.Update(0.16f);
                return _singleParticleConcreteEmitter._particles[0].Velocity.X;
            }

            [Benchmark]
            public float SingleParticleInterfaces()
            {
                _singeParticleInterfaceEmitter.Update(0.16f);
                return _singeParticleInterfaceEmitter._particles[0].Velocity.X;
            }

            [Benchmark]
            public float SingleParticleMultipleIteration()
            {
                _singleMultipleIteration.Update(0.16f);
                return _singleMultipleIteration._particles[0].Velocity.X;
            }

            [Benchmark]
            public float SingleParticleInterfacesSingle()
            {
                _singleInterfaceSingleEmitter.Update(0.16f);
                return _singleInterfaceSingleEmitter._particles[0].Velocity.X;
            }
            
        }
        
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Program.Benchmark>();
        }
    }
}