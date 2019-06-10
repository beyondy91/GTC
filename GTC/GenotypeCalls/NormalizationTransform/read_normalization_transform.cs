using System;
using System.IO;
using System.Collections.Generic;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class NormalizationTransform
    {
        public static NormalizationTransform read_normalization_transform(BinaryReader handle)
        {
            /*
            Static helper function to read normalization transform from file handle

            Args:
                handle(BinaryReader) : File handle with position at start of normalization transform entry

            Returns:
                NormalizationTransform object
            */
            handle.ReadBytes(52);
            return new NormalizationTransform(handle);
        }
    }
}
