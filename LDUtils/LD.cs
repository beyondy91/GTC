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

namespace LDUtils
{
    public partial class LDUtils
    {
        public struct LD
        {
            public string SNP { get; set; }
            public int Flip { get; set; }

            public LD(string SNP, int Flip)
            {
                this.SNP = SNP;
                this.Flip = Flip;
            }
        }
    }
}