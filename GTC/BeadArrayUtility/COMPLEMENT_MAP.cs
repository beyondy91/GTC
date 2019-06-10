using System;
using System.IO;
using System.Collections.Generic;

namespace GTCParse
{
    public static partial class BeadArrayUtility
    {
        static Dictionary<char, char> COMPLEMENT_MAP = new Dictionary<char, char> {
            {'A', 'T'},
            {'T', 'A'},
            {'C', 'G'},
            {'G', 'C'},
            {'D', 'D'},
            {'I', 'I'}
        };
    }
}