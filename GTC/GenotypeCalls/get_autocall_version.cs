using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public string get_autocall_version()
        {
            /*
            Returns:
                string: the version of AutoCall used for genotyping
                For example
                    1.6.2.2
            */
            return ((string)__get_generic(GenotypeCalls.__ID_AUTOCALL_VERSION, read_string));
        }
    }
}