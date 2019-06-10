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
        public static List<QCResult> runQC(List<string> snpNames,
            List<string> sampleNames,
            List<int> Sexs,
            List<byte[]> genotypes,
            List<string> Chromosomes,
            int sex_male = 1,
            int sex_female = 2,
            string chromosome_x = "X",
            double cutoff_callrateSample = 0.9,
            double cutoff_callrateSNP = 0.9,
            double cutoff_HWE = 1e-6,
            double cutoff_heterozygosity = 0.2,
            byte genotype_nocall = 0,
            byte genotype_hetero = 2,
            byte genotype_ref_homo = 1,
            byte genotype_eff_homo = 3)
        {
            var Excluded = new Dictionary<string, Dictionary<string, bool>>();
            foreach(string sampleName in sampleNames)
            {
                Excluded[sampleName] = new Dictionary<string, bool>();
                foreach(string snpName in snpNames)
                {
                    Excluded[sampleName][snpName] = false;
                }
            }

            var result = new List<QCResult>();
            int excluded_number;
            while(true)
            {
                excluded_number = 0;
                var qc_result = callrateSample(snpNames: snpNames, 
                    sampleNames: sampleNames, 
                    genotypes: genotypes, 
                    Excluded: Excluded, 
                    cutoff: cutoff_callrateSample,
                    genotype_nocall: genotype_nocall);
                excluded_number += qc_result.Count;
                foreach(string sampleName in qc_result)
                {
                    foreach(string snpName in snpNames)
                    {
                        Excluded[sampleName][snpName] = true;
                        result.Add(new QCResult(snpName: snpName, sn: sampleName, reason: "callrateSample"));
                    }
                }
                qc_result = callrateSNP(snpNames: snpNames, 
                    sampleNames: sampleNames, 
                    genotypes: genotypes, 
                    Excluded: Excluded, 
                    cutoff: cutoff_callrateSNP, 
                    genotype_nocall: genotype_nocall);
                excluded_number += qc_result.Count;
                foreach (string sampleName in sampleNames)
                {
                    foreach (string snpName in qc_result)
                    {
                        Excluded[sampleName][snpName] = true;
                        result.Add(new QCResult(snpName: snpName, sn: sampleName, reason: "callrateSNP"));
                    }
                }
                qc_result = HWE(snpNames: snpNames,
                    sampleNames: sampleNames,
                    genotypes: genotypes,
                    Excluded: Excluded,
                    cutoff: cutoff_HWE,
                    genotype_hetero: genotype_hetero,
                    genotype_ref_homo: genotype_ref_homo,
                    genotype_eff_homo: genotype_eff_homo);
                excluded_number += qc_result.Count;
                foreach (string sampleName in sampleNames)
                {
                    foreach (string snpName in qc_result)
                    {
                        Excluded[sampleName][snpName] = true;
                        result.Add(new QCResult(snpName: snpName, sn: sampleName, reason: "callrateSNP"));
                    }
                }
                qc_result = heterozygosity(snpNames: snpNames,
                    sampleNames: sampleNames,
                    Sexs: Sexs,
                    genotypes: genotypes,
                    Chromosomes: Chromosomes,
                    Excluded: Excluded,
                    sex_male: sex_male,
                    sex_female: sex_female,
                    chromosome_x: chromosome_x,
                    cutoff: cutoff_heterozygosity,
                    genotype_nocall: genotype_nocall,
                    genotype_hetero: genotype_hetero);
                excluded_number += qc_result.Count;
                foreach (string sampleName in qc_result)
                {
                    foreach (string snpName in snpNames)
                    {
                        Excluded[sampleName][snpName] = true;
                        result.Add(new QCResult(snpName: snpName, sn: sampleName, reason: "heterozygosity"));
                    }
                }

                if (excluded_number == 0)
                {
                    break;
                }
            }
            for(int sample_idx = 0; sample_idx < sampleNames.Count; ++sample_idx)
            {
                result.AddRange(snpNames.AsParallel().Select((snpName, snp_idx) =>
                {
                    if (genotypes[sample_idx][snp_idx] == genotype_nocall)
                        return new QCResult(snpName: snpName, sn: sampleNames[sample_idx], reason: "nocall");
                    else
                        return null;
                }).ToList());
            }
            return result;
        }
    }
}
