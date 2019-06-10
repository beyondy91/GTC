using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public string get_snp_manifest()
        {
            /*
            Returns:
                string: The name of the manifest used for genotyping
            */
            return ((string)__get_generic(GenotypeCalls.__ID_SNP_MANIFEST, read_string));
        }
    }
}