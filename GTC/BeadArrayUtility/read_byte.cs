using System;
using System.IO;
using System.Collections.Generic;

namespace GTCParse
{
    public static partial class BeadArrayUtility
    {
        public static object read_byte(BinaryReader handle)
        {
            /*
            Helper function to parse byte from file handle

            Args:
                handle(file) : BinaryReader

            Returns:
                byte
            */
            return ((object)handle.ReadByte());
        }
    }
}