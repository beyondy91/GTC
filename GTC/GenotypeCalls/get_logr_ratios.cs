using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public List<float> get_logr_ratios(int offset = 0, int? count = null)
        {
            /*
            Returns:
                list(float) : The logR ratios
            */
            if (this.version < 4)
                throw new Exception("LogR ratios unavailable in GTC File version (" + this.version + ")");
            return (this.__get_generic_array_numpy(GenotypeCalls.__ID_LOGR_RATIOS, sizeof(float), offset, count).Select(x => BitConverter.ToSingle(x, 0)).ToList());
        }
    }
}