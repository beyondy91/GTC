using System;
using System.IO;
using System.Collections.Generic;

namespace GTCParse
{
    public static partial class BeadArrayUtility
    {
        public static char Complement(char nucleotide)
        {
            /*
    Complement a single nucleotide. Complements of D(eletion) and I(nsertion) are D and I, respectively.

    Args:
        nucleotide (string) : Nucleotide, must be A, C, T, G, D, or I

    Returns:
        str : Complemented nucleotide

    Raises:
        ValueError: Nucleotide must be one of A, C, T, G, D, or I
             */
            if (!COMPLEMENT_MAP.ContainsKey(nucleotide))
            {
                throw new Exception("Nucleotide must be one of A, C, T, G, D, or I");
            }
            return (COMPLEMENT_MAP[nucleotide]);
        }
    }
}