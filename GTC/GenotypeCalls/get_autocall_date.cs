using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public string get_autocall_date()
        {
            /*
            Returns:
                string: The imaging date of autocall execution
                For example
                    2/17/2015 1:47 PM
            */
            return ((string)__get_generic(GenotypeCalls.__ID_AUTOCALL_DATE, read_string));
        }
    }
}