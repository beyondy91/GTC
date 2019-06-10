﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public int get_num_intensity_only()
        {
            /*
            Returns:
                int: The number of intensity only SNPs
            */
            using (BinaryReader gtc_handle = new BinaryReader(this.gtc_stream))
            {
                gtc_handle.BaseStream.Seek(this.toc_table[GenotypeCalls.__ID_GC50] + 12, SeekOrigin.Begin);
                return ((int)read_int(gtc_handle));
            }
        }
    }
}