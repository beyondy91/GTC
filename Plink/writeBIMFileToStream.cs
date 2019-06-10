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
        public void writeBIMFileToStream(StreamWriter sw_bim_file)
        {
            sw_bim_file.Write(this.bimFile);
            sw_bim_file.Flush();
        }
    }
}