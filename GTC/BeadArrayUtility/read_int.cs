using System;
using System.IO;
using System.Collections.Generic;

namespace GTCParse
{
    public static partial class BeadArrayUtility
    {
        public static object read_int(BinaryReader handle)
        {
            /*
            Helper function to parse int from file handle

            Args:
                handle(file) : BinaryReader

            Returns:
                int
            */
            return ((object)handle.ReadInt32());
        }
    }
}