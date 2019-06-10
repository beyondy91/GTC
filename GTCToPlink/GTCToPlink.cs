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

namespace GTCToPlink
{
    public static class GTCToPlink
    {
        public static Plink.Plink ConvertGTCToPlink(List<GenotypeCalls> gtcs, BeadPoolManifest bpm = null)
        {
            Dictionary<char, byte> sex_map = new Dictionary<char, byte>
            {
                {'F', 0x02},
                {'M', 0x01},
                {'U', 0x00}
            };
            List<string> FIDs = gtcs.AsParallel().Select(gtc => gtc.get_sample_name()).ToList();
            List<string> parent = gtcs.AsParallel().Select(gtc => "0").ToList();
            List<byte> Sexs = gtcs.AsParallel().Select(gtc => sex_map[gtc.get_gender()]).ToList();
            List<byte> Phenotypes = gtcs.AsParallel().Select(gtc => (byte)1).ToList();
            int num_snps = gtcs[0].get_genotypes().Length;
            List<byte[]> genotypesGTC = gtcs.AsParallel().Select(gtc => gtc.get_genotypes()).ToList();
            List<byte[]> genotypes = new List<byte[]>();
            for (int snpIdx = 0; snpIdx < num_snps; ++snpIdx)
            {
                genotypes.Add(new byte[gtcs.Count]);
                for (int gtcIdx = 0; gtcIdx < gtcs.Count; ++gtcIdx)
                {
                    genotypes[snpIdx][gtcIdx] = genotypesGTC[gtcIdx][snpIdx];
                }
            }
            Console.WriteLine("GTC data loaded; Converting to Plink");
            return new Plink.Plink(FIDs: FIDs,
                IIDs: FIDs,
                Fathers: parent,
                Mothers: parent,
                Sexs: Sexs,
                Phenotypes: Phenotypes,
                Chromosomes: bpm == null ? null : bpm.chroms,
                snpNames: bpm == null ? null : bpm.names,
                cMs: bpm == null ? null : Enumerable.Range(0, bpm.num_loci).Select(x => 0).ToList(),
                Positions: bpm == null ? null : bpm.map_infos,
                alleles: bpm == null ? null : bpm.snps.AsParallel().Select(allele => allele.Replace("[", "").Replace("]", "").Split('/').ToList()).ToList(),
                genotypes: genotypes,
                strands: bpm == null ? null : bpm.ref_strands.Select(x => Convert.ToByte(x)).ToList());
        }
    }
}
