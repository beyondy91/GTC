using System;
using System.IO;
using System.Collections.Generic;

namespace GTCParse
{
    public static partial class BeadArrayUtility
    {
        public static object read_float(BinaryReader handle)
        {
            /*
            Helper function to parse float from file handle

            Args:
                handle(file) : BinaryReader

            Returns:
                float
            */
            return ((object)handle.ReadSingle());
        }
    }
}