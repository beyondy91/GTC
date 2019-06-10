using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public List<object> __get_generic_array(short toc_entry, Func<BinaryReader, object> parse_function,
                                          int item_size, int offset, int? count)
        {
            /*
            Internal helper function to access a data array in a generic
            fashion.

            Args:
                toc_entry(short) : Identifier for entry in table of contents
                parse_function(function): A function used to parse the value
                                            from a file handle
                item_size(int): Size(in bytes) of individual entry
                offset(uint): Offset(in elements counts) to start reading
                count(int): Number of entries to read(None is read all remaining entries)

            Returns:
                list(object) : An array parsed from the file(type dependent on parse_function)
            */
            List<object> result = new List<object>();
            using (BinaryReader gtc_handle = generic_seeker(toc_entry))
            {
                int num_entries = (int)read_int(gtc_handle) - offset;
                if (count != null)
                {
                    num_entries = Math.Min((Int32)num_entries, (Int32)count);
                }
                if (offset > 0)
                {
                    gtc_handle.BaseStream.Seek(this.toc_table[toc_entry] + 4 + offset * item_size, SeekOrigin.Begin);
                }
                for (int idx = 0; idx < num_entries; ++idx)
                {
                    result.Add(parse_function(gtc_handle));
                }
            }
            return (result);
        }

    }
}