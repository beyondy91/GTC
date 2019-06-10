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
        public List<List<string>> getAlleles(List<string> snpnames)
        {
            if(this.snpMap.Count == 0)
            {
                this.createMap();
            }
            var result = new List<List<string>>();
            for(int idx = 0; idx < snpnames.Count; ++idx)
            {
                var alleles = new List<string> { "", "" };
                if(this.snpMap.ContainsKey(snpnames[idx]))
                {
                    alleles[0] = this.bims[this.snpMap[snpnames[idx]]].Allele1;
                    alleles[1] = this.bims[this.snpMap[snpnames[idx]]].Allele2;
                }
                result.Add(alleles);
            }
            return result;
        }
    }
}