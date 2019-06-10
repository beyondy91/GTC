using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public List<string> get_base_calls_forward_strand(List<string> snps, List<int> forward_strand_annotations)
        {
            /*
            Get base calls on the forward strand.

            Args:
                snps(list(string)) : A list of string representing the snp on the design strand for the loci(e.g. [A / C])
                forward_strand_annotations(list(int)) : A list of strand annotations for the loci(e.g., SourceStrand.Forward)

            Returns:
                The genotype basecalls on the report strand as a list of strings.
                The characters are A, C, G, T, or - for a no-call/null.
            */
            return (this.get_base_calls_generic(snps, forward_strand_annotations, SourceStrand.Forward, RefStrand.Unknown));
        }
    }
}