using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public List<string> get_base_calls_plus_strand(List<string> snps, List<int> ref_strand_annotations)
        {
            /*
            Get base calls on plus strand of genomic reference.If you only see no-calls returned from this method,
           please verify that the reference strand annotations passed as argument are not unknown (RefStrand.Unknown)

           Args:

               snps (list(string)) : A list of string representing the snp on the design strand for the loci(e.g. [A / C])
                ref_strand_annotations(list(int)) : A list of strand annotations for the loci(e.g., RefStrand.Plus)

            Returns:
                list(string) : The genotype basecalls on the report strand
                 The characters are A, C, G, T, or - for a no-call/null.
            */
            return (this.get_base_calls_generic(snps, ref_strand_annotations, RefStrand.Plus, RefStrand.Unknown));
        }
    }
}