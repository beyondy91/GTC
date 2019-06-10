using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public float get_call_rate()
        {
            /*
            Returns:
                float: the call rate
            */
            return ((float)this.__get_generic(GenotypeCalls.__ID_CALL_RATE, read_float));
        }
    }
}