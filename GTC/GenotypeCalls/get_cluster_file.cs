using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public string get_cluster_file()
        {
            /*
            Returns:
                string: The name of the cluster file used for genotyping
            */
            return ((string)__get_generic(GenotypeCalls.__ID_CLUSTER_FILE, read_string));
        }
    }
}