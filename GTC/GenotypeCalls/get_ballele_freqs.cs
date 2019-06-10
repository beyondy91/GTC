using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public List<float> get_ballele_freqs(int offset = 0, int? count = null)
        {
            /*
            Returns:
                list(float) : The B allele frequencies
            */
            if (this.version < 4)
                throw new Exception("B allele frequencies unavailable in GTC File version (" + this.version + ")");
            return (this.__get_generic_array_numpy(GenotypeCalls.__ID_B_ALLELE_FREQS, sizeof(float), offset, count).Select(x => BitConverter.ToSingle(x, 0)).ToList());
        }
    }
}