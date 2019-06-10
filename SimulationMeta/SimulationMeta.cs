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
    public class SimulationMeta
    {
        public string var;
        public double beta, freq;

        public SimulationMeta(double beta, double freq, string var)
        {
            this.beta = beta;
            this.freq = freq;
            this.var = var;
        }

        public SimulationMeta(JObject jObject)
        {
            this.beta = Convert.ToDouble(jObject["beta"]);
            this.freq = Convert.ToDouble(jObject["freq"]);
            this.var = Convert.ToString(jObject["var"]);
        }

        public JObject ToJObject()
        {
            var result = new JObject();
            result["beta"] = this.beta;
            result["freq"] = this.freq;
            result["var"] = this.var;
            return result;
        }
    }
}
