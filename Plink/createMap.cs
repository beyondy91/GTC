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
        public void createMap()
        {
            this.snpMap = new Dictionary<string, int>();
            for(int idx = 0; idx < this.bims.Count; ++idx)
            {
                this.snpMap[this.bims[idx].snpName] = idx;
            }
        }
    }
}