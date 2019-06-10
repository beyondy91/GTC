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
        public void writePlinkToStream(StreamWriter sw_fam = null,
            StreamWriter sw_bim = null,
            StreamWriter sw_bim_file = null,
            BinaryWriter bw_bed = null,
            BinaryWriter bw_strand = null)
        {
            if(sw_fam != null) 
                this.writeFAMToStream(sw_fam);
            if(sw_bim != null)
                this.writeBIMToStream(sw_bim);
            if (sw_bim_file != null)
                this.writeBIMFileToStream(sw_bim_file);
            if (bw_bed != null)
                this.writeBEDToStream(bw_bed);
            if(bw_strand != null)
                this.writeStrandToStream(bw_strand);
        }
    }
}