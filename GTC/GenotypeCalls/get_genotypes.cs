using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public byte[] get_genotypes(int offset = 0, int? count = null)
        {
            /*
            Returns:
                string: A byte list(string) of genotypes.See code2genotype for mapping
            */
            List<object> result = __get_generic_array(GenotypeCalls.__ID_GENOTYPES, read_byte, 1, offset, count);
            byte[] result_byte = new byte[result.Count];
            for (int i = 0; i < result.Count; ++i)
            {
                result_byte[i] = (byte)result[i];
            }
            return (result_byte);
        }


    }
}