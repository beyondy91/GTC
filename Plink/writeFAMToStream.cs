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
        public void writeFAMToStream(StreamWriter sw_fam)
        {
            foreach(FAM fam in this.fams)
            {
                sw_fam.WriteLine(String.Join(' ', new object[] { fam.FID, fam.IID, fam.Father, fam.Mother, fam.Sex, fam.Phenotype }));
            }
            sw_fam.Flush();
        }
    }
}