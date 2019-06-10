using System;
using System.IO;
using System.Collections.Generic;

namespace GTCParse
{
    public static partial class BeadArrayUtility
    {
        public static string read_string(BinaryReader handle)
        {
            /*
            Helper function to parse string from file handle.See https://msdn.microsoft.com/en-us/library/yzxa6408(v=vs.100).aspx
            for additional details on string format.

            Args:
                handle (file): BinaryReader

            Returns:
                string

            Raises:
                Exception: Failed to read complete string
            */

            // python ver. code 

            /*
            int total_length = 0;
            byte partial_length = handle.ReadByte();
            int num_bytes = 0;
            while ((partial_length & 0x80) > 0){
                total_length += (partial_length & 0x7F) << (7 * num_bytes);
                partial_length = handle.ReadByte();
                ++num_bytes;
            }

            total_length += partial_length << (7 * num_bytes);
            string result = new string(handle.ReadChars(total_length));
            if (result.Length < total_length)
            {
                throw new Exception("Failed to read complete string");
            }
            return new string(result);
            */
            return (handle.ReadString());
        }
    }
}
