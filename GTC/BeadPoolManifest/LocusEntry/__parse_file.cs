using System;
using System.IO;
using System.Collections.Generic;

namespace GTCParse
{
    public partial class LocusEntry
    {
        private void __parse_file(BinaryReader handle)
        {
            /*
            Helper function to initialize this object from a file handle

            Args:
                handle(file handle) : File handle at start of locus entry record

            Returns:
                None
            */
            this.version = (int)BeadArrayUtility.read_int(handle);
            if (this.version == 6)
                this.__parse_locus_version_6(handle);
            else if (this.version == 7)
                this.__parse_locus_version_7(handle);
            else if (this.version == 8)
                this.__parse_locus_version_8(handle);
            else
                throw new Exception("Manifest format error: unknown version for locus entry (" + this.version + ")");
        }
    }
}