using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace GTCParse.BeadPoolManifestAmend
{
    public partial class LocusEntryWriter
    {
        public static void Writer(BinaryWriter bw, LocusEntry locus_entry)
        {
            bw.Write(locus_entry.version);
            if (locus_entry.version == 6)
                write_locus_version_6(bw, locus_entry);
            else if (locus_entry.version == 7)
                write_locus_version_7(bw, locus_entry);
            else if (locus_entry.version == 8)
                write_locus_version_8(bw, locus_entry);
            else
                throw new Exception("Manifest format error: unknown version for locus entry (" + locus_entry.version + ")");

        }
    }
}
