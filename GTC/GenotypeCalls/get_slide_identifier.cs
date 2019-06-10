using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public string get_slide_identifier()
        {
            /*
            Returns:
                string: The sentrix identifier for the slide
            */
            return ((string)__get_generic(GenotypeCalls.__ID_SLIDE_IDENTIFIER, read_string));
        }
    }
}