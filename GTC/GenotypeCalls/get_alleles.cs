using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public List<char[]> get_alleles()
        {
            /*
            Returns:
                list(char[]): (ref allele, eff allele) set for each allele.
            */
            int ploidy_type;
            try
            {
                ploidy_type = this.get_ploidy_type();
            }
            catch
            {
                ploidy_type = 1;
            }

            if (ploidy_type != 1)
            {

            }

            byte[] genotypes;
            if (ploidy_type != 1)
            {
                genotypes = this.get_genotypes();
            }
            else
            {
                genotypes = new byte[1];
            }

            List<char[]> result = new List<char[]>();
            using (BinaryReader gtc_handle = new BinaryReader(this.gtc_stream))
            {
                gtc_handle.BaseStream.Seek(this.toc_table[GenotypeCalls.__ID_BASE_CALLS], SeekOrigin.Begin);
                int num_entries = (int)read_int(gtc_handle);
                for (int idx = 0; idx < num_entries; ++idx)
                {
                    result.Add(gtc_handle.ReadChars(2));
                }

            }
            return (result);
        }
    }
}