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
        public static List<char> getAllele(string allele_string)
        {
            List<char> alleles = allele_string.Replace("[", "").Replace("]", "").Split('/').Select(x => x[0]).ToList();
            return alleles;
        }
    }
}