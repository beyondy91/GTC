using System;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenomeChain
{
    public class Chain
    {
        public List<ChainRecord> chainRecords;
        public Chain(Stream stream)
        {
            this.chainRecords = new List<ChainRecord>();
            StreamReader sr = new StreamReader(stream);
            while(!sr.EndOfStream)
            {
                this.chainRecords.Add(ChainRecord.createChainRecord(sr: sr));
            }
            this.chainRecords = this.chainRecords.Where(x => x != null).ToList();
        }
    }
}
