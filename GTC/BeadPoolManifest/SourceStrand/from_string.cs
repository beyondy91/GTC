using System;
namespace GTCParse
{
    public partial class SourceStrand
    {
        public static int from_string(string source_strand)
        {
            /*Get an integer representation of source strand annotation

            Args:
                source_strand(int) : int representation of source strand annotation(e.g., "F")

            Returns:
                int : int representation of source strand annotation(e.g.SourceStrand.Forward)

            Raises:
                ValueError: Unexpected value for source strand
    */
            if (source_strand == "U" || source_strand == "") return SourceStrand.Unknown;
            else if (source_strand == "F") return SourceStrand.Forward;
            else if (source_strand == "R") return SourceStrand.Reverse;
            else throw new Exception("Unexpected value for source strand " + source_strand);
        }
    }
}
