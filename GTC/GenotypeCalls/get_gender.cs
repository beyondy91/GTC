using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public char get_gender()
        {
            /*
            Returns:
                char: the gender
                M - Male, F - Female, U-Unknown
            */
            return ((char)this.__get_generic(GenotypeCalls.__ID_GENDER, read_char));
        }
    }
}