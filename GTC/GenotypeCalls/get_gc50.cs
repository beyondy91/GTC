using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public float get_gc50()
        {
            /*
            Returns:
                float: the GC50(GenCall score - 50th percentile)
            */
            return ((float)this.__get_generic(GenotypeCalls.__ID_GC50, read_float));
        }

    }
}