using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public List<byte[]> __get_generic_array_numpy(short toc_entry, int type_size, int offset = 0, int? count = null)
        {
            /*
            Internal helper function to access a data array in a generic
            fashion.

            Args:
                toc_entry(short) : Identifier for entry in table of contents
                type_size(int): Size of data type to read into array
                offset(int) : Offset(in element counts) to start reading
               count(int): Number of entries to read(None will read remaining entries)

            Returns:
                list(object) : An array parsed from the file(need to be casted to type)
            */
            List<byte[]> result = new List<byte[]>();
            using (BinaryReader gtc_handle = generic_seeker(toc_entry))
            {
                int num_entries = (int)read_int(gtc_handle) - offset;
                if (count != null)
                {
                    num_entries = Math.Min((Int32)num_entries, (Int32)count);
                }
                int item_size = type_size;
                if (offset > 0)
                {
                    gtc_handle.BaseStream.Seek(this.toc_table[toc_entry] + 4 + offset * item_size, SeekOrigin.Begin);
                }

                for (int idx = 0; idx < num_entries; ++idx)
                {
                    result.Add(gtc_handle.ReadBytes(item_size));
                }
            }
            return (result);
        }
    }
}