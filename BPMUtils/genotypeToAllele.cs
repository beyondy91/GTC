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
        public static string genotypeToAllele(int genotype, List<string> allele)
        {
            string res = "";
            if (genotype > 0)
            {
                for (int idx = 0; idx < 3 - genotype; ++idx)
                    res += allele[0];
                for (int idx = 0; idx < genotype - 1; ++idx)
                    res += allele[1];
            }
            char[] alleles = res.ToCharArray();
            Array.Sort(alleles);
            res = "";
            foreach (char ch in alleles)
                res += ch.ToString();
            return res;
        }
    }
}
