using System;
using System.IO;
using System.Collections.Generic;

namespace GTCParse
{


    public partial class LocusEntry
    {
        private void __parse_locus_version_7(BinaryReader handle)
        {
            /*
            Helper function to parse version 7 locus entry

            Args:
                handle(file) : File handle at start of locus entry record

            Returns:
                None
            */
            this.__parse_locus_version_6(handle);
            this.after_ver6_byte = handle.ReadBytes(4 * 4);
        }
    }
}