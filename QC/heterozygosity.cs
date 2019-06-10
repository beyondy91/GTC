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
        public static List<string> heterozygosity(List<string> snpNames,
            List<string> sampleNames,
            List<byte[]> genotypes,
            List<int> Sexs,
            List<string> Chromosomes,
            Dictionary<string, Dictionary<string, bool>> Excluded,
            int sex_male = 1,
            int sex_female = 2,
            string chromosome_x = "X",
            double cutoff = 0.2,
            byte genotype_nocall = 0,
            byte genotype_hetero = 2)
        {
            return sampleNames.AsParallel().Select((sampleName, sample_idx) =>
            {
                var total = snpNames.Select((snpName, snp_idx) => Convert.ToInt32(!Excluded[sampleName][snpName] && Chromosomes[snp_idx] == chromosome_x && genotypes[sample_idx][snp_idx] != genotype_nocall)).ToList().Sum();
                if (total == 0)
                    return null;
                var hetero = snpNames.Select((snpName, snp_idx) => Convert.ToInt32(!Excluded[sampleName][snpName] && Chromosomes[snp_idx] == chromosome_x && genotypes[sample_idx][snp_idx] == genotype_hetero)).ToList().Sum();
                if (hetero * 1.0 / total > cutoff && Sexs[sample_idx] == sex_female)
                    return null;
                else if (hetero * 1.0 / total < cutoff && Sexs[sample_idx] == sex_male)
                    return null;
                else
                    return sampleName;
            }).ToList();
        }
    }
}
