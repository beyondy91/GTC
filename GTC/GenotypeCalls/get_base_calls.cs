using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public List<string> get_base_calls()
        {
            /*
            Returns:
                list(string): The genotype basecalls
                The characters are A, C, G, T, or - for a no-call/null.
                The calls are relative to the top strand.
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

            List<string> result = new List<string>();
            using (BinaryReader gtc_handle = new BinaryReader(this.gtc_stream))
            {
                gtc_handle.BaseStream.Seek(this.toc_table[GenotypeCalls.__ID_BASE_CALLS], SeekOrigin.Begin);
                int num_entries = (int)read_int(gtc_handle);
                for (int idx = 0; idx < num_entries; ++idx)
                {
                    if (ploidy_type == 1)
                    {
                        result.Add(new string(gtc_handle.ReadChars(2)));
                    }
                    else
                    {
                        byte[] byte_string = gtc_handle.ReadBytes(2);
                        string ab_genotype = code2genotype[genotypes[idx]];
                        if (ab_genotype == "NC" || ab_genotype == "NULL") result.Add("-");
                        else
                        {
                            string top_genotype = "";
                            for (int i = 0; i < ab_genotype.Length; ++i)
                            {
                                char allele = ab_genotype[i];
                                if (allele == 'A') top_genotype += (char)byte_string[0];
                                else top_genotype += (char)byte_string[1];
                            }
                            result.Add(top_genotype);
                        }
                    }
                }

            }
            return (result);
        }

    }
}