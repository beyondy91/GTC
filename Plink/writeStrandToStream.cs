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
        public void writeStrandToStream(BinaryWriter bw_strand)
        {
            bw_strand.Write(this.strands.ToArray());
            bw_strand.Flush();
        }
    }
}