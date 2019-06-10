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
        public static Dictionary<string, List<string>> alleleBPM(List<string> names, List<List<string>> Alleles)
        {
            var allele_mapping = new Dictionary<string, List<string>>();
            for (int idx = 0; idx < names.Count; ++idx)
            {
                allele_mapping[names[idx]] = Alleles[idx];
            }

            return allele_mapping;
        }
    }
}