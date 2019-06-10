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
        public void writeBIMToStream(StreamWriter sw_bim)
        {
            writeBIMToStream(sw_bim: sw_bim, bims: this.bims);
        }

        public static void writeBIMToStream(StreamWriter sw_bim, List<BIM> bims)
        {
            foreach (BIM bim in bims)
            {
                sw_bim.WriteLine(String.Join(' ', new object[] { bim.Chromosome, bim.snpName, bim.cM, bim.Position, bim.Allele1, bim.Allele2 }));
            }
            sw_bim.Flush();
        }
    }
}