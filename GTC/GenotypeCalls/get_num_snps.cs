using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public int get_num_snps()
        {
            /*
            Returns:
                int: The number of SNPs in the file
            */
            return (this.toc_table[GenotypeCalls.__ID_NUM_SNPS]);
        }
    }
}