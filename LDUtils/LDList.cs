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

        public static Dictionary<string, List<LD>> LDList(string ld_file)
        {
            Dictionary<string, List<LD>> res = new Dictionary<string, List<LD>>();

            using (FileStream fs = new FileStream(ld_file, FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                {
                    List<string> line = sr.ReadLine().Split(',').ToList();
                    if (!res.Keys.Contains(line[0]))
                        res[line[0]] = new List<LD>();
                    res[line[0]].Add(new LD(line[1], Convert.ToInt32(line[2])));
                }
            }
            return res;
        }
    }
}