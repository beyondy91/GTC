using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace GTCParse.BeadPoolManifestAmend
{
    public partial class LocusEntryWriter
    {
        public static void write_locus_version_6(BinaryWriter bw, LocusEntry locus_entry)
        {
            bw.Write(locus_entry.ilmn_id);
            bw.Write(locus_entry.name);
            foreach (string str in locus_entry.after_name_string1)
                bw.Write(str);
            bw.Write(locus_entry.after_name_byte);
            foreach (string str in locus_entry.after_name_string2)
                bw.Write(str);
            bw.Write(locus_entry.snp);
            bw.Write(locus_entry.chrom);
            foreach (string str in locus_entry.after_chrom_string)
                bw.Write(str);
            bw.Write(locus_entry.map_info.ToString());
            foreach (string str in locus_entry.after_map_info_string)
                bw.Write(str);
            bw.Write(locus_entry.address_a);
            bw.Write(locus_entry.address_b);
            foreach (string str in locus_entry.after_address_string)
                bw.Write(str);
            bw.Write(locus_entry.after_address_byte);
            bw.Write((byte)locus_entry.assay_type);
        }
    }
}