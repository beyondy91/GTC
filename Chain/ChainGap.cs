using System;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenomeChain
{
    public class ChainGap
    {
        public int size; // the size of the ungapped alignment
        public int dt; // the difference between the end of this block and the beginning of the next block(reference sequence)
        public int dq; // the difference between the end of this block and the beginning of the next block(query sequence)
        public ChainGap(int size,
            int dt = 0,
            int dq = 0)
        {
            this.size = size;
            this.dt = dt;
            this.dq = dq;
        }
    }
}
