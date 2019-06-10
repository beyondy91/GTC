using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace QC
{
    public class QCResult
    {
        // reason: callrateSample, callrateSNP, HWE, heterozygosity
        public string sn, snpName, reason;
        public QCResult (string sn, string snpName, string reason)
        {
            this.sn = sn;
            this.snpName = snpName;
            this.reason = reason;
        }
    }
}