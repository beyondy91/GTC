using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public float get_logr_dev()
        {
            /*
            Returns:
                float: the logR deviation
            */
            return ((float)this.__get_generic(GenotypeCalls.__ID_LOGR_DEV, read_float));
        }

    }
}