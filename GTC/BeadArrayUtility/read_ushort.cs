using System;
using System.IO;
using System.Collections.Generic;

namespace GTCParse
{
    public static partial class BeadArrayUtility
    {
        public static object read_ushort(BinaryReader handle)
        {
            /*
            Helper function to parse ushort from file handle

            Args:
                handle(file) : BinaryReader

            Returns:
                ushort
            */
            return ((object)handle.ReadUInt16());
        }
    }
}