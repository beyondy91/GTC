using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public List<int> get_raw_y_intensities(int offset = 0, int? count = null)
        {
            /*
            Returns:
                list(int) : The raw y intensities of assay bead types
            */
            return (this.__get_generic_array_numpy(GenotypeCalls.__ID_RAW_Y, sizeof(ushort), offset, count).Select(x => Convert.ToInt32(BitConverter.ToUInt16(x, 0))).ToList());
        }
    }
}