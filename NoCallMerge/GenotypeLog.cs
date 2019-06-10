using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace NoCallMerge
{
    public class GenotypeLog
    {
        public string sn, snpName;
        public GenotypeLog (string sn, string snpName)
        {
            this.sn = sn;
            this.snpName = snpName;
        }
    }
}