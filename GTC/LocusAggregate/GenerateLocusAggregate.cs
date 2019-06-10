using System;
using System.Collections.Generic;
using System.Linq;
namespace GTCParse
{
    public class GenerateLocusAggregate
    {
        /*
        Function object to generate aggregate data for a given
        locus
        */
        public List<LocusAggregate> buffer;
        public int relative_offset;

        public GenerateLocusAggregate(List<LocusAggregate> buffer, int relative_offset)
        {
            /*
            Constructor

            Args:
                buffer(list(LocusAggregate)): List where each element is slice of locus data from single sample

               relative_offset(int): Offset that was used when loading buffer

              Returns:
                    GenerateLocusAggregate
            */
            this.buffer = buffer;
            this.relative_offset = relative_offset;
        }

        public LocusAggregate __call__ (int locus_idx) {
            /*
            Create a new LocusAggregate representing data from a single locus across all samples

            Args:
                locus_idx(int): Global locus index(will be automatically adjusted by relative offset in constructor)


            Returns:
            LocusAggregate: Data for a single locus aggregated across all samples
            */

            LocusAggregate locus_aggregate = new LocusAggregate();

            int relative_locus_idx = locus_idx - this.relative_offset;
            foreach(LocusAggregate sample_buffer in this.buffer)
            {
                locus_aggregate.genotypes.Add(
                    sample_buffer.genotypes[relative_locus_idx]);
                locus_aggregate.b_allele_freqs.Add(
                        sample_buffer.b_allele_freqs[relative_locus_idx]);
                locus_aggregate.log_r_ratios.Add(
                        sample_buffer.log_r_ratios[relative_locus_idx]);
                locus_aggregate.x_intensities.Add(
                        sample_buffer.x_intensities[relative_locus_idx]);
                locus_aggregate.y_intensities.Add(
                        sample_buffer.y_intensities[relative_locus_idx]);
                locus_aggregate.transforms.Add(
                        sample_buffer.transforms[relative_locus_idx]);

            }
            return (locus_aggregate);
        }
    }
}
