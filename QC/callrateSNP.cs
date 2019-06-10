using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace QC
{
    public static partial class QC
    {
        public static List<string> callrateSNP(List<string> snpNames,
            List<string> sampleNames,
            List<byte[]> genotypes,
            Dictionary<string, Dictionary<string, bool>> Excluded,
            double cutoff = 0.9,
            byte genotype_nocall = 0)
        {
            return snpNames.AsParallel().Select((snpName, snp_idx) =>
            {
                var total = sampleNames.Select((sampleName, sample_idx) => Convert.ToInt32(!Excluded[sampleName][snpName])).ToList().Sum();
                if (total == 0)
                    return null;
                var call = sampleNames.Select((sampleName, sample_idx) => Convert.ToInt32(!Excluded[sampleName][snpName] && genotypes[sample_idx][snp_idx] != genotype_nocall)).ToList().Sum();
                if (call * 1.0 / total > cutoff)
                    return null;
                else
                    return snpName;
            }).ToList();
        }
    }
}
