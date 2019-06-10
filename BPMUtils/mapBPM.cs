using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using GTCParse;

namespace BPMUtils
{
    public partial class Utils
    {
        public static Dictionary<string, Dictionary<int, List<string>>> mapBPM(List<string> names,
            List<string> Chromosomes,
            List<int> Positions)
        {
            var mapping = new Dictionary<string, Dictionary<int, List<string>>>();
            for (int idx = 0; idx < names.Count; ++idx)
            {
                if (!mapping.ContainsKey(Chromosomes[idx]))
                    mapping[Chromosomes[idx]] = new Dictionary<int, List<string>>();
                if (!mapping[Chromosomes[idx]].ContainsKey(Positions[idx]))
                    mapping[Chromosomes[idx]][Positions[idx]] = new List<string>();
                mapping[Chromosomes[idx]][Positions[idx]].Add(names[idx]);
            }

            return mapping;
        }
    }
}