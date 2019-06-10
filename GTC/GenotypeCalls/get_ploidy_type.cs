using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public int get_ploidy_type()
        {
            /*
            Returns:
                int: The ploidy type of the sample
            */
            return (this.toc_table[GenotypeCalls.__ID_PLOIDY_TYPE]);
        }
    }
}