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
        public byte[] checkRepeatSingle(string snpname)
        {
            if (!this.snpMap.ContainsKey(snpname))
            {
                return this.genotypes[0].AsParallel().Select(genotype => (byte)0).ToArray();
            }
            int snpIdx = this.snpMap[snpname];
            BIM bim = this.bims[snpIdx];
            if(!this.coordMap.ContainsKey(bim.Chromosome))
            {
                return this.genotypes[snpIdx];
            }
            byte[] result = this.genotypes[snpIdx];
            if (bim.Chromosome != "0")
            {
                List<int> repeatIdxs = this.coordMap[bim.Chromosome][bim.Position];
                foreach (int repeatIdx in repeatIdxs)
                {
                    for (int snIdx = 0; snIdx < result.Length; ++snIdx)
                    {
                        if (result[snIdx] == 0 && this.genotypes[repeatIdx][snIdx] > 0)
                        {
                            if (this.bims[repeatIdx].Allele1 == this.bims[snpIdx].Allele1 && this.bims[repeatIdx].Allele2 == this.bims[snpIdx].Allele2)
                            {
                                result[snIdx] = this.genotypes[repeatIdx][snIdx];
                            }
                            else if (this.bims[repeatIdx].Allele1 == this.bims[snpIdx].Allele2 && this.bims[repeatIdx].Allele2 == this.bims[snpIdx].Allele1)
                            {
                                result[snIdx] = (byte)(4 - this.genotypes[repeatIdx][snIdx]);
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}