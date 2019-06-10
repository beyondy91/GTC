using System;
using System.IO;
using System.Collections.Generic;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class BeadPoolManifest
    {
        private void __parse_file(BinaryReader manifest_handle)
        {
            /*
            Helper function to initialize this object from a file.

            Args:
                manifest_file (string): Location of BPM (bead pool manifest) file

            Returns:
                None

            Raises:
                Exception: Unsupported or unknown BPM version
                Exception: Manifest format error
            */
            string header = new string(manifest_handle.ReadChars(3));
            if (header.Length != 3 || header != "BPM")
                throw new Exception("Invalid BPM format");
            this.version = (int)(byte)read_byte(manifest_handle);
            if (this.version != 1)
                throw new Exception("Unknown BPM version (" + this.version + ")");
            this.version = (int)read_int(manifest_handle);
            int version_flag = 0x1000;
            if ((this.version & version_flag) == version_flag)
                this.version = (this.version ^ version_flag);
            if (this.version > 5 || this.version < 3)
                throw new Exception("Unsupported BPM version (" + this.version + ")");
            this.manifest_name = read_string(manifest_handle);
            if (version > 1)
                this.control_config = read_string(manifest_handle);
            this.num_loci = (int)read_int(manifest_handle);
            this.after_num_loci = manifest_handle.ReadBytes(4 * this.num_loci);
            this.name_lookup = new Dictionary<string, int>();
            for (int idx = 0; idx < this.num_loci; ++idx)
            {
                this.names.Add(read_string(manifest_handle));
                name_lookup[this.names[this.names.Count - 1]] = idx;
            }
            for (int idx = 0; idx < this.num_loci; ++idx)
            {
                int normalization_id = (int)(byte)read_byte(manifest_handle);
                if (normalization_id >= 100)
                    throw new Exception("Manifest format error: read invalid normalization ID");
                this.normalization_ids.Add(normalization_id);
            }

            for (int i = 0; i < this.num_loci; ++i)
            {
                this.assay_types.Add(0);
                this.addresses.Add(0);
                this.snps.Add("");
                this.chroms.Add("");
                this.map_infos.Add(0);
                this.ref_strands.Add(RefStrand.Unknown);
                this.source_strands.Add(RefStrand.Unknown);
                this.locus_entries.Add(null);
            }

            for (int idx = 0; idx < this.num_loci; ++idx)
            {
                LocusEntry locus_entry = new LocusEntry(manifest_handle);
                this.assay_types[name_lookup[locus_entry.name]] = locus_entry.assay_type;
                this.addresses[name_lookup[locus_entry.name]] = locus_entry.address_a;
                this.snps[name_lookup[locus_entry.name]] = locus_entry.snp;
                this.chroms[name_lookup[locus_entry.name]] = locus_entry.chrom;
                this.map_infos[name_lookup[locus_entry.name]] = locus_entry.map_info;
                this.ref_strands[name_lookup[locus_entry.name]] = locus_entry.ref_strand;
                this.source_strands[name_lookup[locus_entry.name]] = locus_entry.source_strand;
                this.locus_entries[name_lookup[locus_entry.name]] = locus_entry;
            }

            if (this.normalization_ids.Count != this.assay_types.Count)
                throw new Exception("Manifest format error: read invalid number of assay entries");

            this.all_norm_ids = new HashSet<int>();
            for (int locus_idx = 0; locus_idx < this.num_loci; ++locus_idx)
            {
                this.normalization_ids[locus_idx] += 100 * this.assay_types[locus_idx];
                all_norm_ids.Add(this.normalization_ids[locus_idx]);
            }

            List<int> sorted_all_norm_ids = new List<int>(all_norm_ids);
            sorted_all_norm_ids.Sort();

            this.lookup_dictionary = new Dictionary<int, int>();
            for (int idx = 0; idx < all_norm_ids.Count; ++idx)
            {
                lookup_dictionary[sorted_all_norm_ids[idx]] = idx;
            }

            foreach (int normalization_id in this.normalization_ids)
                this.normalization_lookups.Add(lookup_dictionary[normalization_id]);
        }
    }
}