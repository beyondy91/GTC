using System;
using System.IO;
using System.Collections.Generic;

namespace GTCParse
{


    public partial class LocusEntry
    {
        /*
        Helper class representing a locus entry within a bead pool manifest.Current only support version
        6,7, and 8.

        Attributes:
            ilmn_id (string) : IlmnID (probe identifier) of locus
            name (string): Name (variant identifier) of locus
            snp (string) : SNP value for locus (e.g., [A / C])
            chrom (string) : Chromosome for the locus (e.g., XY)
            map_info (int) : Mapping location of locus
            assay_type (int) : Identifies type of assay (0 - Infinium II, 1 - Infinium I (A/T), 2 - Infinium I(G/C)
            address_a(int) : AddressA ID of locus
            address_b(int) : AddressB ID of locus(0 if none)
            ref_strand(int) : See RefStrand class
           source_strand (int) : See SourceStrand class
        */
        public string ilmn_id, name, snp, chrom;
        public int map_info, assay_type, address_a, address_b, ref_strand, source_strand, version;
        public List<string> after_name_string1, after_name_string2, after_chrom_string, after_map_info_string, after_address_string;
        public byte[] after_name_byte, after_address_byte, after_ver6_byte;

        public LocusEntry(BinaryReader handle)
        {

            /*
            Constructor

            Args:
                handle(file):  File handle at start of locus entry record

            Returns:
                LocusEntry
            */
            this.ilmn_id = "";
            this.name = "";
            this.snp = "";
            this.chrom = "";
            this.map_info = -1;
            this.assay_type = -1;
            this.address_a = -1;
            this.address_b = -1;
            this.ref_strand = RefStrand.Unknown;
            this.source_strand = SourceStrand.Unknown;
            this.version = 0;
            this.after_name_string1 = new List<string>();
            this.after_name_string2 = new List<string>();
            this.after_chrom_string = new List<string>();
            this.after_map_info_string = new List<string>();
            this.after_address_string = new List<string>();
            this.__parse_file(handle);
        }
    }
}
