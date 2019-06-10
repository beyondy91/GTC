using System;
using System.IO;
using System.Collections.Generic;

namespace GTCParse
{
    public static partial class BeadArrayUtility
    {
        public static object read_char(BinaryReader handle)
        {
            /*
            Helper function to parse character from file handle

            Args:
                handle(file) : BinaryReader

            Returns:
                char
            */
            return ((object)(char)handle.ReadByte());
        }
    }
}