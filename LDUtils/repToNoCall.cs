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

namespace LDUtils
{
    public partial class LDUtils
    {
        public static List<byte> repToNoCall(Dictionary<string, Dictionary<int, List<string>>> map,
            Dictionary<string, List<string>> allele_map,
            Dictionary<string, int> name_map,
            string chr,
            int pos,
            List<byte[]> genotype_calls,
            IEnumerable<int> num_snp,
            int snp,
            List<byte> strands)
        {
            return genotype_calls
                .AsParallel()
                .Select<byte[], byte>((genotype_call, genotype_idx) =>
                {
                    int snp_idx = name_map[map[chr][pos][snp]];
                    string snp_name = map[chr][pos][snp];
                    if (snp_idx >= genotype_call.Length)
                        return 0;
                    if (genotype_call[snp_idx] != 0)
                        return genotype_call[snp_idx];
                    List<char> alleles = allele_map[snp_name].Select(x => x[0]).ToList();
                    byte strand = strands[snp_idx];
                    List<List<char>> alt_alleles = num_snp.Select(alt_snp => allele_map[map[chr][pos][alt_snp]].Select(x => x[0]).ToList()).ToList();
                    List<byte> alt_strands = num_snp.Select(alt_snp => strands[name_map[map[chr][pos][alt_snp]]]).ToList();
                    List<byte> alt_call = num_snp.Select(alt_snp =>
                    {
                        int alt_snp_idx = name_map[map[chr][pos][alt_snp]];
                        if (genotype_call.Length > alt_snp_idx)
                            return genotype_call[alt_snp_idx];
                        else
                            return (byte)0;
                    }).ToList();
                    foreach (int idx in num_snp)
                    {
                        if (alt_call[idx] > 0)
                        {
                            if (strand == alt_strands[idx])
                            {
                                // if strand of snp is same with the strand of repeating snp
                                if (alleles[0] == alt_alleles[idx][0])
                                {
                                    // if reference allele matches
                                    return alt_call[idx];
                                }
                                else
                                {
                                    // if reference allele does NOT match
                                    return (byte)(4 - alt_call[idx]);
                                }
                            }
                            else
                            {
                                // if strand of snp is NOT same with the strand of repeating snp
                                if (alleles[0] == getComplement(alt_alleles[idx][0]))
                                {
                                    // if reference allele matches
                                    return alt_call[idx];
                                }
                                else
                                {
                                    // if reference allele does NOT match
                                    return (byte)(4 - alt_call[idx]);
                                }
                            }
                        }
                    }
                    return (byte)0;
                }).ToList();
        }
    }
}