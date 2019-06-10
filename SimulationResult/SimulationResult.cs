using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json.Linq;


namespace Simulations
{
    public class SimulationResult
    {
        public int size;
        public float mean, exp_mean;
        public float[] result;

        public SimulationResult(float[] result)
        {
            this.size = result.Length;
            this.mean = result.Sum() / this.size;
            this.exp_mean = result.AsParallel().Select(x => Convert.ToSingle(Math.Exp(x - this.mean))).Sum() / this.size;
            this.result = result;
        }

        public SimulationResult(BinaryReader br)
        {
            this.size = br.ReadInt32();
            this.mean = br.ReadSingle();
            this.exp_mean = br.ReadSingle();
            this.result = new float[this.size];
            for(int idx = 0; idx < this.size; ++idx)
            {
                this.result[idx] = br.ReadSingle();
            }
        }

        public MemoryStream ToStream()
        {
            var stream = new MemoryStream();
            var bw = new BinaryWriter(stream);
            bw.Write(this.size);
            bw.Write(this.mean);
            bw.Write(this.exp_mean);
            for(int idx = 0; idx < this.size; ++idx)
            {
                bw.Write(this.result[idx]);
            }
            bw.Flush();
            return stream;
        }
    }
}