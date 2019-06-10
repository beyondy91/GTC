using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using GTCParse;

namespace BPMUtils
{
    public partial class Utils
    {
        public static Dictionary<char, char> Complement_map = new Dictionary<char, char>
        {
            {'A', 'T'},
            {'T', 'A'},
            {'C', 'G'},
            {'G', 'C'},
            {'D', 'D'},
            {'I', 'I'},
            {'-', 'D'}
        };
    }
}