using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MathNet.Numerics.Distributions;

namespace QC
{
    public static partial class QC
    {
        public static List<string> HWE(List<string> snpNames,
            List<string> sampleNames,
            List<byte[]> genotypes,
            Dictionary<string, Dictionary<string, bool>> Excluded,
            string chromosome_x = "X",
            double cutoff=1e-6,
            byte genotype_hetero = 2,
            byte genotype_ref_homo = 1,
            byte genotype_eff_homo = 3)
        {
            ChiSquared chi = new ChiSquared(1);
            double hwe_chisq_cutoff = chi.InverseCumulativeDistribution(1 - cutoff);
            return snpNames.AsParallel().Select((snpName, snp_idx) =>
            {
                int ref_homo, eff_homo, hetero, n;
                double p, q;
                double exp_ref_homo, exp_eff_homo, exp_hetero;
                double hwe_chisq;
                ref_homo = sampleNames.Select((sampleName, sample_idx) => Convert.ToInt32(!Excluded[sampleName][snpName] && genotypes[sample_idx][snp_idx] == genotype_ref_homo)).ToList().Sum();
                eff_homo = sampleNames.Select((sampleName, sample_idx) => Convert.ToInt32(!Excluded[sampleName][snpName] && genotypes[sample_idx][snp_idx] == genotype_eff_homo)).ToList().Sum();
                hetero = sampleNames.Select((sampleName, sample_idx) => Convert.ToInt32(!Excluded[sampleName][snpName] && genotypes[sample_idx][snp_idx] == genotype_hetero)).ToList().Sum();
                n = ref_homo + eff_homo + hetero;
                if (n == 0)
                    return null;
                p = (2 * eff_homo + hetero) / (double)(2 * n);
                q = 1 - p;
                exp_eff_homo = p * p * n + float.Epsilon;
                exp_ref_homo = q * q * n + float.Epsilon;
                exp_hetero = 2 * p * q * n + float.Epsilon;
                hwe_chisq = Math.Pow((eff_homo - exp_eff_homo), 2) / exp_eff_homo +
                          Math.Pow((hetero - exp_hetero), 2) / exp_hetero +
                          Math.Pow((ref_homo - exp_ref_homo), 2) / exp_ref_homo;
                if (hwe_chisq_cutoff < hwe_chisq)
                    return snpName;
                else
                    return null;
            }).ToList();
        }
    }
}
