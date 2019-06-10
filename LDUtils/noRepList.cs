using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using GTCParse;
using static BPMUtils.Utils;
using static GTCUtils.GTCUtils;

namespace LDUtils
{
    public partial class LDUtils
    {

        public static List<string> noRepList(string rep_file)
        {
            List<string> res = new List<string>();
            using (FileStream fs = new FileStream(rep_file, FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                {
                    res.Add(sr.ReadLine());
                }
            }
            return res;
        }
    }
}