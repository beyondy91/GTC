using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace GTCParse.BeadPoolManifestAmend
{
    public partial class LocusEntryWriter
    {
        public static void write_locus_version_8(BinaryWriter bw, LocusEntry locus_entry)
        {
            write_locus_version_7(bw, locus_entry);
            bw.Write(RefStrand.to_string(locus_entry.ref_strand));
        }
    }
}
