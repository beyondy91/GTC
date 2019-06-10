using System;
using System.IO;
using System.Collections.Generic;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class BeadPoolManifest
    {
        public static string genotypeToAllele(byte genotype, List<string> allele)
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
