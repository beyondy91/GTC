using System;
using System.IO;
using System.Collections.Generic;

namespace GTCParse
{


    public partial class LocusEntry
    {
        private void __parse_locus_version_8(BinaryReader handle)
        {
            /*
            Helper function to parse version 8 locus entry

            Args:
                handle(file) : File handle at start of locus entry record

            Returns:
                None
            */
            this.__parse_locus_version_7(handle);
            this.ref_strand = RefStrand.from_string(BeadArrayUtility.read_string(handle));
        }

    }
}
