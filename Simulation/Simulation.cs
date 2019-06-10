using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MathNet.Numerics.Distributions;

namespace Simulations
{
    public static partial class Simulation
    {
        public static float[] RunSimulation(List<SimulationMeta> metas,
            int size=1000000)
        {
            float[] result = new float[size];

            for(int idx = 0; idx < result.Length; ++idx)
            {
                result[idx] = (float)0.0;
            }

            Console.WriteLine("size: " + result.Length);

            double[] normal_sample = new double[size];

            foreach(SimulationMeta meta in metas)
            {
                Normal.Samples(normal_sample, meta.freq * meta.beta, Math.Sqrt(meta.beta * meta.freq * (1-meta.freq)));
                Console.WriteLine("freq: " + meta.freq);
                Console.WriteLine("beta: " + meta.beta);
                Console.WriteLine("mean: " + meta.freq * meta.beta);
                Console.WriteLine("sd: " + Math.Sqrt(meta.beta * meta.freq * (1 - meta.freq)));
                Console.WriteLine("sample: " + normal_sample[0]);
                for(int idx = 0; idx < result.Length; ++idx)
                {
                    if(!double.IsNaN(normal_sample[idx]))
                        result[idx] += (float)normal_sample[idx];
                }
                Console.WriteLine("result: " + result[0]);
            }

            return result;
        }
    }
}
