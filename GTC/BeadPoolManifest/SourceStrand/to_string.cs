using System;
namespace GTCParse
{
    public partial class SourceStrand
    {
        public static string to_string(int source_strand)
        {
            /*
            Get a string representation of source strand annotation

            Args:
                source_strand(int) : int representation of source strand(e.g., SourceStrand.Forward)

            Returns:
                str : string representation of source strand annotation

            Raises:
                ValueError: Unexpected value for source strand
            */
            if (source_strand == SourceStrand.Unknown) return "U";
            else if (source_strand == SourceStrand.Forward) return "F";
            else if (source_strand == SourceStrand.Reverse) return "R";
            else throw new Exception("Unexpected value for source strand " + source_strand);
        }
    }
}
