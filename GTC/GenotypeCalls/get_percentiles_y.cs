using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public List<ushort> get_percentiles_y()
        {
            /*
            Returns:
                list(ushort): An array of length three representing 5th, 50th and 95th percentiles for
                y intensity
            */
            if (this.version < 5)
                throw new Exception("Percentile intensities unavailable in GTC File version (" + this.version + ")");
            List<ushort> result = new List<ushort>();
            using (BinaryReader gtc_handle = new BinaryReader(this.gtc_stream))
            {
                gtc_handle.BaseStream.Seek(this.toc_table[GenotypeCalls.__ID_PERCENTILES_Y], SeekOrigin.Begin);
                for (int idx = 0; idx < 3; ++idx)
                    result.Add((ushort)read_ushort(gtc_handle));
            }
            return (result);
        }
    }
}