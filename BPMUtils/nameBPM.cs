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
        public static Dictionary<string, int> nameBPM(List<string> names)
        {
            var name_mapping = new Dictionary<string, int>();

            for (int idx = 0; idx < names.Count; ++idx)
            {
                name_mapping[names[idx]] = idx;
            }

            return name_mapping;
        }
    }
}