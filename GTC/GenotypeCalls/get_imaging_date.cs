using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public string get_imaging_date()
        {
            /*
            Returns:
                string: The imaging date of scanning
                For example
                    Monday, December 01, 2014 4:51:47 PM
            */
            return ((string)__get_generic(GenotypeCalls.__ID_IMAGING_DATE, read_string));
        }
    }
}