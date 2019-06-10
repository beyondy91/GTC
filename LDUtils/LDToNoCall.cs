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
        public static List<byte> LDToNoCall(Dictionary<string, Dictionary<int, List<string>>> map,
            Dictionary<string, List<string>> allele_map,
            Dictionary<string, int> name_map,
            string chr,
            int pos,
            List<byte[]> gtc_genotype_calls,
            IEnumerable<int> num_snp,
            int snp,
            Dictionary<string, List<LD>> ld)
        {
            string snp_name = map[chr][pos][snp];
            int snp_idx = name_map[snp_name];
            return gtc_genotype_calls
                .AsParallel()
                .Select<byte[], byte>(genotype_call =>
                {
                    if (snp_idx > genotype_call.Length)
                        return (byte)0;
                    if (genotype_call[snp_idx] == 0)
                    {
                        List<LD> lds = ld[snp_name];
                        foreach (LD ld_snp in lds)
                        {
                            int ld_snp_idx = name_map[ld_snp.SNP];
                            if (genotype_call.Length > ld_snp_idx && genotype_call[ld_snp_idx] != 0)
                            {
                                if (ld_snp.Flip == 0)
                                {
                                    return (byte)(4 - genotype_call[ld_snp_idx]);
                                }
                                else
                                {
                                    return genotype_call[ld_snp_idx];
                                }
                            }
                        }
                    }
                    return genotype_call[snp_idx];
                })
                .ToList();
        }
    }
}
