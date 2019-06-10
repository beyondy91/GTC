using System;
using System.IO;
using System.Collections.Generic;

namespace GTCParse
{
    public partial class LocusEntry
    {
        private void __parse_locus_version_6(BinaryReader handle)
        {
            /*
            Helper function to parse version 6 locus entry

            Args:
                handle(file) : File handle at start of locus entry record

            Returns:
                None

            Raises:
                Exception: Manifest format error
            */
            this.ilmn_id = BeadArrayUtility.read_string(handle);
            this.source_strand = SourceStrand.from_string(this.ilmn_id.Split('_')[this.ilmn_id.Split('_').Length - 2]);
            this.name = BeadArrayUtility.read_string(handle);
            for (int idx = 0; idx < 3; ++idx)
                this.after_name_string1.Add(BeadArrayUtility.read_string(handle));
            this.after_name_byte = handle.ReadBytes(4);
            for (int idx = 0; idx < 2; ++idx)
                this.after_name_string2.Add(BeadArrayUtility.read_string(handle));
            this.snp = BeadArrayUtility.read_string(handle);
            this.chrom = BeadArrayUtility.read_string(handle);
            for (int idx = 0; idx < 2; ++idx)
                this.after_chrom_string.Add(BeadArrayUtility.read_string(handle));
            this.map_info = Int32.Parse(BeadArrayUtility.read_string(handle));
            for (int idx = 0; idx < 2; ++idx)
                this.after_map_info_string.Add(BeadArrayUtility.read_string(handle));
            this.address_a = (int)BeadArrayUtility.read_int(handle);
            this.address_b = (int)BeadArrayUtility.read_int(handle);
            for (int idx = 0; idx < 7; ++idx)
                this.after_address_string.Add(BeadArrayUtility.read_string(handle));
            this.after_address_byte = handle.ReadBytes(3);
            this.assay_type = (int)(byte)BeadArrayUtility.read_byte(handle);
            if (!(new List<int>(new int[] { 0, 1, 2 }).Contains(this.assay_type)))
                throw new Exception("Format error in reading assay type from locus entry");
            if (this.address_b == 0)
            {
                if (this.assay_type != 0)
                    throw new Exception("Manifest format error: Assay type is inconsistent with address B");
            }
            else
            {
                if (this.assay_type == 0)
                    throw new Exception("Manifest format error: Assay type is inconsistent with address B");
            }
        }
    }
}