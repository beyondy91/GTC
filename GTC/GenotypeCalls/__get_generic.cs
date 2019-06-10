using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public object __get_generic(short toc_entry, Func<BinaryReader, object> parse_function)
        {
            /*
                    Internal helper function to access a data element in a
                    generic fashion.

                    Args:
                        toc_entry(short) : Identifier for entry in table of contents
                        parse_function(function): Used to parse the value
                                                    from a file handle
                   Returns:
                        A single value parsed from the file(type dependent on
                        parse_function)
            */
            object result;
            using (BinaryReader gtc_handle = generic_seeker(toc_entry))
            {
                result = parse_function(gtc_handle);
            }
            return (result);
        }


    }
}