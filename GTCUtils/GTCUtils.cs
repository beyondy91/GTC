using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using GTCParse;

namespace GTCUtils
{
    public class GTCUtils
    {
        public static List<string> getFilePaths(string DIR, string GTC)
        {
            List<string> filePaths = new List<string>();
            if (DIR.Length > 0)
            {
                foreach (string dir in DIR.Split(','))
                {
                    filePaths.AddRange(Directory.GetFiles(dir, "*.gtc",
                        SearchOption.AllDirectories).ToList());
                }
            }
            else
            {
                foreach (string gtc_file in GTC.Split(','))
                {
                    filePaths.Add(gtc_file);
                }
            }
            filePaths.Reverse();
            return filePaths;
        }
    }
}
