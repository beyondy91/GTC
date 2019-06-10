using System;
namespace GTCParse
{
    public partial class RefStrand
    {
        public static string to_string(int ref_strand)
        {
            /*
            Get a string reprensetation of ref strand annotation

            Args:
                ref_strand (int) : int representation of ref strand (e.g., RefStrand.Plus)

            Returns:
                str : string representation of reference strand annotation

            Raises:
                ValueError: Unexpected value for reference strand
             */
            if (ref_strand == RefStrand.Unknown)
            {
                return "U";
            }
            else if (ref_strand == RefStrand.Plus)
            {
                return "+";
            }
            else if (ref_strand == RefStrand.Minus)
            {
                return "-";
            }
            else
            {
                throw new Exception("Unexpected value for reference strand " + ref_strand);
            }
        }
    }
}
