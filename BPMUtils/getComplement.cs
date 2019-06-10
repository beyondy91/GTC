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
        public static char getComplement(char allele)
        {
            return Complement_map[allele];
        }
    }
}