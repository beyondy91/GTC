using System;
using System.IO;
using System.Collections.Generic;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class BeadPoolManifest
    {
        /*
            Class for parsing binary(BPM) manifest file.

            Attributes:
            names (list(strings): Names of loci from manifest
           snps (list(strings): SNP values of loci from manifest

           chroms (list(strings): Chromosome values for loci
           map_infos (list(int)): Map infor values for loci
           addresses(list(int)): AddressA IDs of loci from manifest
            normalization_lookups(list(int)) : Normalization lookups from manifest.This indexes into
                                             list of normalization transforms read from GTC file
            ref_strands (list(int)): Reference strand annotation for loci(see RefStrand class)
            source_strands(list(int)) : Source strand annotations for loci(see SourceStrand class)
            num_loci(int) : Number of loci in manifest
            manifest_name(string): Name of manifest
           control_config(string): Control description from manifest
        */
        public List<string> names, snps, chroms;
        public List<int> map_infos, addresses, normalization_lookups, ref_strands, source_strands, normalization_ids, assay_types;
        public string manifest_name, control_config;
        public int num_loci, version;
        public List<LocusEntry> locus_entries;
        public byte[] after_num_loci;
        public Dictionary<string, int> name_lookup;
        public Dictionary<int, int> lookup_dictionary;
        public HashSet<int> all_norm_ids;

        public BeadPoolManifest(BinaryReader br)
        {
            /*
                Constructor

                Args:
                    filename(string): Locations of BPM(bead pool manifest) file

              Returns:
                    BeadPoolManifest
            */
            this.names = new List<string>();
            this.snps = new List<string>();
            this.chroms = new List<string>();
            this.map_infos = new List<int>();
            this.addresses = new List<int>();
            this.normalization_ids = new List<int>();
            this.assay_types = new List<int>();
            this.normalization_lookups = new List<int>();
            this.ref_strands = new List<int>();
            this.source_strands = new List<int>();
            this.num_loci = 0;
            this.manifest_name = "";
            this.control_config = "";
            this.locus_entries = new List<LocusEntry>();
            this.__parse_file(br);
        }
    }
}
