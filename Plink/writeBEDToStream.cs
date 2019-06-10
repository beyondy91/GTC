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
        public void writeBEDToStream(BinaryWriter bw_bed)
        {
            int num_samples = this.fams.Count;

            this.beds = Enumerable.Range(0, this.num_snps).AsParallel().Select(snp_idx =>
            {
                byte[] binary_genotype = new byte[this.byte_per_block];
                for (int j = 0; j < this.byte_per_block; ++j)
                {
                    binary_genotype[j] = (byte)0x00;
                }
                for (int j = 0; j < num_samples; ++j)
                {
                    // slide left 6 bits for 4th, 4 bits for 3rd, 2 bits for 2nd samples
                    int slide = (j % 4) * 2;
                    byte gen;
                    if (snp_idx < genotypes.Count)
                    {
                        gen = genotypes[snp_idx][j];
                        if (genotypes[snp_idx][j] == 0)
                            gen = 0x01;
                        else if (genotypes[snp_idx][j] == 1)
                            gen = 0x00;
                    }
                    else
                    {
                        gen = 0x01;
                    }
                    binary_genotype[j / 4] += (byte)(gen << slide);
                }
                return new BED(Genotype: binary_genotype.ToList());
            }).ToList();

            bw_bed.Write(new byte[] { 0x6c, 0x1b, 0x01 });
            foreach(BED bed in this.beds)
            {
                bw_bed.Write(bed.Genotype.ToArray());
            }
            bw_bed.Flush();
        }
    }
}