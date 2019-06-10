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
        public void createCoordMap()
        {
            this.coordMap = new Dictionary<string, Dictionary<int, List<int>>>();
            for(int idx = 0; idx < this.bims.Count; ++idx)
            {
                string chrom = this.bims[idx].Chromosome;
                int position = this.bims[idx].Position;
                if(!this.coordMap.ContainsKey(chrom))
                {
                    this.coordMap[chrom] = new Dictionary<int, List<int>>();
                }
                if (!this.coordMap[chrom].ContainsKey(position))
                {
                    this.coordMap[chrom][position] = new List<int>();
                }
                this.coordMap[chrom][position].Add(idx);
            }
        }
    }
}