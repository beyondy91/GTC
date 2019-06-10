using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace GTCParse
{
    public partial class ClusterFile
    {
        public static List<object> read_array(BinaryReader handle, int num_entries, Func<BinaryReader, object> read_record)
        {
            /*
            Helper method for reading an array of data from a file handle

            Args:
                handle(file) : File handle
                num_entries(int) : Number of entries to read
                read_record(func(file)): Function(taking file handle as argument) to read a single entry

              Returns:
                    list(object) : A list of objects returned by read_record function
            */
            List<object> result = new List<object>();
            for (int idx = 0; idx < num_entries; ++idx)
                result.Add(read_record(handle));
            return (result);
        }
    }
}