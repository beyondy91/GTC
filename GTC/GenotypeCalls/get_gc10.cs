using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public float get_gc10()
        {
            /*
            Returns:
                float: the GC10(GenCall score - 10th percentile)
            */
            return ((float)this.__get_generic(GenotypeCalls.__ID_GC10, read_float));
        }
    }
}