using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public List<string> get_genotype_base_calls(BeadPoolManifest manifest)
        {
            /*
            Returns:
                list(string): The basecalls from get_genotypes() result
                The characters are A, C, G, T, or - for a no-call/null.
                The calls are relative to the top strand.
            */

            byte[] genotypes;
            genotypes = this.get_genotypes();

            List<string> result = genotypes.AsParallel().Select((genotype, genotype_idx) => BeadPoolManifest.genotypeToAllele(genotype, manifest.snps[genotype_idx].Replace("[", "").Replace("]", "").Split('/').ToList())).ToList();

            return (result);
        }
    }
}