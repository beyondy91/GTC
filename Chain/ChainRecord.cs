using System;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenomeChain
{
    public class ChainRecord
    {
        public ulong score; // chain score
        public string tName; // chromosome (reference sequence)
        public int tSize; // chromosome size (reference sequence)
        public string tStrand; // strand (reference sequence)
        public int tStart; // alignment start position(reference sequence)
        public int tEnd; // alignment end position(reference sequence)
        public string qName; // chromosome(query sequence)
        public int qSize; // chromosome size(query sequence)
        public string qStrand; // strand(query sequence)
        public int qStart; // alignment start position(query sequence)
        public int qEnd; // alignment end position(query sequence)
        public string id; // chain ID
        public List<ChainGap> chainGaps;
        public static ChainRecord createChainRecord(StreamReader sr)
        {
            var chainRecord = new ChainRecord();
            List<string> header = new List<string>();
            while (header.Count < 12 && !sr.EndOfStream)
            {
                header = sr.ReadLine().Split(null).ToList();
            }
            if (header.Count < 12)
            {
                return null;
            }
            header.RemoveAt(0);

            chainRecord.score = Convert.ToUInt64(header[0]);
            chainRecord.tName = header[1];
            chainRecord.tSize = Convert.ToInt32(header[2]);
            chainRecord.tStrand = header[3];
            chainRecord.tStart = Convert.ToInt32(header[4]);
            chainRecord.tEnd = Convert.ToInt32(header[5]);
            chainRecord.qName = header[6];
            chainRecord.qSize = Convert.ToInt32(header[7]);
            chainRecord.qStrand = header[8];
            chainRecord.qStart = Convert.ToInt32(header[9]);
            chainRecord.qEnd = Convert.ToInt32(header[10]);
            chainRecord.id = header[11];

            chainRecord.chainGaps = new List<ChainGap>();
            List<string> gapLine = new List<string>();
            while (true)
            {
                gapLine = sr.ReadLine().Split(null).ToList();
                if(gapLine.Count > 1)
                {
                    chainRecord.chainGaps.Add(new ChainGap(size: Convert.ToInt32(gapLine[0]),
                        dt: Convert.ToInt32(gapLine[1]),
                        dq: Convert.ToInt32(gapLine[2])));
                }
                else
                {
                    chainRecord.chainGaps.Add(new ChainGap(size: Convert.ToInt32(gapLine[0]),
                        dt: 0,
                        dq: 0));
                    break;
                }
            }
            return chainRecord;
        }
    }
}
