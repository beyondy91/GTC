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
        public static List<BIM> readBIMFromStream(StreamReader sr_bim)
        {
            List<BIM> bims = new List<BIM>();
            while(!sr_bim.EndOfStream)
            {
                List<string> row = sr_bim.ReadLine().Split(null).ToList();
                bims.Add(new BIM(Chromosome: row[0],
                    snpName: row[1],
                    cM: Convert.ToInt32(row[2]),
                    Position: Convert.ToInt32(row[3]),
                    Allele1: row[4],
                    Allele2: row[5]));
            }
            return bims;
        }
    }
}