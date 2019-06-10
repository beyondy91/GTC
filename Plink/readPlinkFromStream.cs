using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Plink
{
    public partial class Plink
    {
        public static Plink readPlinkFromStream(StreamReader sr_fam = null, StreamReader sr_bim = null, BinaryReader br_bed = null, BinaryReader br_strand = null)
        {
            List<string> FIDs = new List<string>();
            List<string> IIDs = new List<string>();
            List<string> Fathers = new List<string>();
            List<string> Mothers = new List<string>();
            List<byte> Sexs = new List<byte>();
            List<byte> Phenotypes = new List<byte>();
            if (sr_fam == null)
            {
                FIDs.Add("sample");
                IIDs.Add("sample");
                Fathers.Add("0");
                Mothers.Add("0");
                Sexs.Add(0);
                Phenotypes.Add(0);
            }
            else
            {
                while (!sr_fam.EndOfStream)
                {
                    List<string> row = sr_fam.ReadLine().Split(null).ToList();
                    FIDs.Add(row[0]);
                    IIDs.Add(row[1]);
                    Fathers.Add(row[2]);
                    Mothers.Add(row[3]);
                    Sexs.Add(Convert.ToByte(row[4]));
                    Phenotypes.Add(Convert.ToByte(row[5]));
                }
            }
            int num_samples = FIDs.Count;

            List<string> Chromosomes = new List<string>();
            List<string> snpNames = new List<string>();
            List<int> cMs = new List<int>();
            List<int> Positions = new List<int>();
            List<List<string>> alleles = new List<List<string>>();
            while(!sr_bim.EndOfStream)
            {
                List<string> row = sr_bim.ReadLine().Split(null).ToList();
                Chromosomes.Add(row[0]);
                snpNames.Add(row[1]);
                cMs.Add(Convert.ToInt32(row[2]));
                Positions.Add(Convert.ToInt32(row[3]));
                alleles.Add(new List<string>() {row[4], row[5]});
            }
            int num_snps = Chromosomes.Count;

            List<byte[]> bed_genotypes = new List<byte[]>();
            int byte_per_block = (num_samples + 3) / 4;

            br_bed.ReadBytes(3);
            for (int snp_idx = 0; snp_idx < num_snps; ++snp_idx)
            {
                if (byte_per_block == 1)
                    bed_genotypes.Add(new byte[] { br_bed.ReadByte() });
                else
                    bed_genotypes.Add(br_bed.ReadBytes(byte_per_block));
            }

            List<byte[]> genotypes = bed_genotypes.Select(bed_genotype =>
           {
               byte[] genotype = new byte[num_samples];
               for(int genotype_idx = 0; genotype_idx < num_samples; ++genotype_idx)
               {
                   int bed_idx = genotype_idx / 4;
                   int slide = (genotype_idx % 4) * 2;
                   byte genotype_from_bed = (byte) ((bed_genotype[bed_idx] >> slide) - ((bed_genotype[bed_idx] >> (slide + 2)) << (slide + 2)));
                   if(genotype_from_bed == 0x00)
                   {
                       genotype[genotype_idx] = 0x01;
                   }
                   else if(genotype_from_bed == 0x01)
                   {
                       genotype[genotype_idx] = 0x00;
                   }
                   else
                   {
                       genotype[genotype_idx] = genotype_from_bed;
                   }
               }
               return genotype;
           }).ToList();
            List<byte> strands;
            if(br_strand != null)
            {
                br_strand.BaseStream.Seek(0, SeekOrigin.Begin);
                strands = br_strand.ReadBytes(num_snps).ToList();
            }
            else
            {
                strands = new List<byte>(num_snps);
                for(int snp_idx = 0; snp_idx < strands.Count; ++snp_idx)
                {
                    strands[snp_idx] = 0x01;
                }
            }
            return new Plink(FIDs: FIDs,
                IIDs: IIDs,
                Fathers: Fathers,
                Mothers: Mothers,
                Sexs: Sexs,
                Phenotypes: Phenotypes,
                Chromosomes: Chromosomes,
                snpNames: snpNames,
                cMs: cMs,
                Positions: Positions,
                alleles: alleles,
                genotypes: genotypes,
                strands: strands);
        }
    }
}