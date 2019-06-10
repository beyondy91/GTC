using System;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plink;

namespace GenomeChain
{
    public class LiftOver
    {
        public static BIM bimLiftOver(Chain chain,
            BIM input)
        {
            string chr = input.Chromosome;
            int pos = input.Position;
            if(pos > 0)
            {
                foreach (ChainRecord chainRecord in chain.chainRecords)
                {
                    if (chainRecord.tName == ("chr" + chr) &&
                        chainRecord.tStart <= pos &&
                        chainRecord.tEnd >= pos)
                    {
                        int tStart = chainRecord.tStart, qStart = chainRecord.qStart;
                        for (int idx = 0; idx < chainRecord.chainGaps.Count; ++idx)
                        {
                            var chainGap = chainRecord.chainGaps[idx];
                            if (pos < tStart + chainGap.size)
                            {
                                input.Position = pos + (qStart - tStart);
                                break;
                            }
                            tStart += chainGap.size + chainGap.dt;
                            qStart += chainGap.size + chainGap.dq;
                        }
                        break;
                    }
                }
            }
            return input;
        }
    }
}
