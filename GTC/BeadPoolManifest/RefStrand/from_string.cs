using System;
namespace GTCParse
{
    public partial class RefStrand
    {
        public static int from_string(string ref_strand)
        {
            /*Get an integer representation of ref strand annotation

            Args:
                ref_strand (str) : string representation of reference strand annotation (e.g., "+")

            Returns:
                int : int representation of reference strand annotation (e.g. RefStrand.Plus)

            Raises:
                ValueError: Unexpected value for reference strand
            */
            if (ref_strand == "U" || ref_strand == "")
            {
                return RefStrand.Unknown;
            }
            else if (ref_strand == "+")
            {
                return RefStrand.Plus;
            }
            else if (ref_strand == "-")
            {
                return RefStrand.Minus;
            }
            else
            {
                throw new Exception("Unexpected value for reference strand " + ref_strand);
            }
        }
    }
}
