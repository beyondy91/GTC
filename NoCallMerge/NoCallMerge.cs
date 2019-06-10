using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using GTCParse;
using static BPMUtils.Utils;
using static GTCUtils.GTCUtils;
using static LDUtils.LDUtils;
using Plink;

namespace NoCallMerge
{
    public static class NoCallMerge
    {
        public static Dictionary<string, Object> RunNoCallMerge(List<string> Chromosomes,
            List<string> snpNames,
            List<int> Positions,
            List<List<string>> Alleles,
            List<byte> strands,
            List<string> sampleNames,
            List<byte[]> genotype_calls,
            string OUT = "OUT",
            Dictionary<string, List<LD>> LD_FILE = null,
            string LD_FILL = "LD_FILL",
            List<string> NOREP_FILE = null,
            string REP_FILL = "REP_FILL",
            List<string> SNP_FILE = null,
            string NOCALL = "NOCALL",
            string SN = "",
            List<string> REMOVE_FILE = null,
            string REMOVE_FILL = "REMOVE_FILL")
        {
            Dictionary<string, Object> merge_result = new Dictionary<string, Object>();
            var map = mapBPM(names: snpNames, Chromosomes: Chromosomes, Positions: Positions);
            var allele_map = alleleBPM(names: snpNames, Alleles: Alleles);
            var name_map = nameBPM(names: snpNames);

            var norep = NOREP_FILE.Where(x => name_map.Keys.Contains(x)).ToList();
            var ld = LD_FILE;
            var out_snp = SNP_FILE;
            var remove_snp = REMOVE_FILE;

            var REMOVE_FILL_LIST = new List<GenotypeLog>();
            foreach (string snp in remove_snp)
            {
                if (name_map.Keys.Contains(snp))
                {
                    int snp_idx = name_map[snp];
                    for (int idx = 0; idx < genotype_calls.Count; ++idx)
                    {
                        if (snp_idx < genotype_calls[idx].Length)
                        {
                            if (out_snp.Contains(snp))
                            {
                                REMOVE_FILL_LIST.Add(new GenotypeLog(sn: sampleNames[idx], snpName: snpNames[snp_idx]));
                            }
                            genotype_calls[idx][snp_idx] = (byte)0;
                        }
                    }
                }
            }
            merge_result[REMOVE_FILL] = REMOVE_FILL_LIST;

            var NOCALL_LIST = new List<GenotypeLog>();
            var REP_FILL_LIST = new List<GenotypeLog>();
            var LD_FILL_LIST = new List<GenotypeLog>();

            Console.WriteLine("Searching repeating SNPs");

            foreach (string chr in map.Keys)
            {
                foreach (int pos in map[chr].Keys)
                {
                    IEnumerable<int> num_snp = Enumerable.Range(0, map[chr][pos].Count);
                    foreach (int x in num_snp)
                    {
                        List<byte> genotype;
                        string snp_name = map[chr][pos][x];
                        int snp_idx = name_map[snp_name];
                        if (!norep.Contains(snp_name))
                        {
                            genotype = repToNoCall(map, allele_map, name_map, chr, pos, genotype_calls, num_snp, x, strands);
                            for (int idx = 0; idx < genotype.Count; ++idx)
                            {
                                if (genotype_calls[idx].Length > snp_idx && genotype_calls[idx][snp_idx] != genotype[idx])
                                {
                                    if (out_snp.Contains(snp_name))
                                    {
                                        REP_FILL_LIST.Add(new GenotypeLog(sn: sampleNames[idx], snpName: snp_name));
                                    }
                                    genotype_calls[idx][snp_idx] = genotype[idx];
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Searching LD SNPs");
            foreach (string chr in map.Keys)
            {
                foreach (int pos in map[chr].Keys)
                {
                    IEnumerable<int> num_snp = Enumerable.Range(0, map[chr][pos].Count);
                    foreach (int x in num_snp)
                    {
                        List<byte> genotype;
                        string snp_name = map[chr][pos][x];
                        int snp_idx = name_map[snp_name];
                        if (out_snp.Contains(snp_name))
                        {
                            if (ld.Keys.Contains(snp_name))
                            {
                                genotype = LDToNoCall(map, allele_map, name_map, chr, pos, genotype_calls, num_snp, x, ld);
                            }
                            else
                            {
                                genotype = genotype_calls.AsParallel().Select(genotype_call =>
                                {
                                    if (genotype_call.Length > snp_idx)
                                        return genotype_call[snp_idx];
                                    else
                                        return (byte)0;
                                }).ToList();
                            }

                            for (int idx = 0; idx < genotype.Count; ++idx)
                            {
                                if (genotype_calls[idx].Length > snp_idx && genotype_calls[idx][snp_idx] != genotype[idx])
                                {
                                    if (out_snp.Contains(snp_name))
                                    {
                                        LD_FILL_LIST.Add(new GenotypeLog(sn: sampleNames[idx], snpName: snp_name));
                                    }
                                    genotype_calls[idx][snp_idx] = genotype[idx];
                                }
                            }


                            var NOCALL_SNP = genotype
                                .AsParallel()
                                .Select((y, idx) =>
                                {
                                    if (y == 0)
                                        return new GenotypeLog(sn: sampleNames[idx], snpName: snp_name);
                                    else
                                        return null;
                                })
                                .ToList();
                            NOCALL_LIST.AddRange(NOCALL_SNP);
                        }
                    }
                }
            }
            merge_result[NOCALL] = NOCALL_LIST;
            merge_result[REP_FILL] = REP_FILL_LIST;
            merge_result[LD_FILL] = LD_FILL_LIST;

            return (merge_result);
        }
    }
}
