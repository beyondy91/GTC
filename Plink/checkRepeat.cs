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
        public List<byte[]> checkRepeat(List<string> snpnames)
        { 
            this.createMap();
            this.createCoordMap();
            return snpnames.Select((snpname, snpIdx) =>
            {
                return this.checkRepeatSingle(snpname: snpname);
            }).ToList();
        }
    }
}