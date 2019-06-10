using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTCParse.BeadPoolManifestAmend
{
    public static partial class BeadPoolManifestAmend
    {
        public static MemoryStreamNotDispose beadPoolManifestWriter(BeadPoolManifest bpm)
        {
            MemoryStreamNotDispose fs = new MemoryStreamNotDispose();
            using (BinaryWriter bw = new BinaryWriter(fs))
            {

                for (int locus_idx = 0; locus_idx < bpm.num_loci; ++locus_idx)
                {
                    bpm.normalization_ids[locus_idx] -= 100 * bpm.assay_types[locus_idx];
                }



                bw.Write(System.Text.Encoding.ASCII.GetBytes("BPM"));
                bw.Write((byte)1);
                bw.Write(bpm.version);
                bw.Write(bpm.manifest_name);
                bw.Write(bpm.control_config);
                bw.Write(bpm.num_loci);
                bw.Write(bpm.after_num_loci);
                for(int idx = 0; idx < bpm.num_loci; ++idx)
                {
                    bw.Write(bpm.names[idx]);
                }
                for(int idx = 0; idx < bpm.num_loci; ++idx)
                {
                    bw.Write((byte)bpm.normalization_ids[idx]);
                }

                for (int idx = 0; idx < bpm.num_loci; ++idx)
                {
                    LocusEntryWriter.Writer(bw, bpm.locus_entries[idx]);
                }
            }
            return fs;
        }
    }
}
