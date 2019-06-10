using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public List<string> get_base_calls_generic(List<string> snps, List<int> strand_annotations, int report_strand, int unknown_annotation)
        {
            /*
            Get base calls on arbitrary strand

            Args:
                snps(list(string)) : A list of string representing the snp on the design strand for the loci(e.g. [A / C])
                strand_annotations(list(int)) : A list of strand annotations for the loci
                report_strand(int) : The strand to use for reporting(must match encoding of strand_annotations)
                unknown_annotation(int) : The encoding used in strand annotations for an unknown strand

           Returns:
                list(string) : The genotype basecalls on the report strand
                 The characters are A, C, G, T, or - for a no-call/null.

            Raises:
                ValueError: Number of SNPs or ref strand annotations not matched to entries in GTC file
            */
            byte[] genotypes = this.get_genotypes();

            if (genotypes.Length != snps.Count)
            {
                throw new Exception("The number of SNPs must match the number of loci in the GTC file");
            }

            if (genotypes.Length != strand_annotations.Count)
            {
                throw new Exception("The number of reference strand annotations must match the number of loci in the GTC file");
            }

            List<string> result = new List<string>();

            byte genotype;
            string snp;
            int strand_annotation;
            string ab_genotype;
            char a_nucleotide;
            char b_nucleotide;
            for (int i = 0; i < genotypes.Length; ++i)
            {
                genotype = genotypes[i];
                snp = snps[i];
                strand_annotation = strand_annotations[i];
                ab_genotype = code2genotype[genotype];
                a_nucleotide = snp[1];
                b_nucleotide = snp[snp.Length - 2];
                if (a_nucleotide == 'N' || b_nucleotide == 'N' || strand_annotation == unknown_annotation || ab_genotype == "NC" || ab_genotype == "NULL")
                {
                    result.Add("-");
                }
                else
                {
                    List<char> report_strand_nucleotides = new List<char>();
                    char ab_allele, nucleotide_allele;
                    for (int j = 0; j < ab_genotype.Length; ++j)
                    {
                        ab_allele = ab_genotype[j];
                        if (ab_allele == 'A') nucleotide_allele = a_nucleotide;
                        else nucleotide_allele = b_nucleotide;
                        if (strand_annotation == report_strand) report_strand_nucleotides.Add(nucleotide_allele);
                        else report_strand_nucleotides.Add(Complement(nucleotide_allele));
                    }
                    result.Add(new string(report_strand_nucleotides.ToArray()));
                }
            }
            return (result);
        }
    }
}