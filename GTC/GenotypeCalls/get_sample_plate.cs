using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public string get_sample_plate()
        {
            /*
            Returns:
                string: The name of the sample plate
            */
            return ((string)__get_generic(GenotypeCalls.__ID_SAMPLE_PLATE, read_string));
        }
    }
}