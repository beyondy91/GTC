using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace GTCParse
{
    public class Loader
    {

        /*
        A function object which handles loading a contiguous slice of loci
        data for a given sample
        */
        public int locus_offset, loci_buffer_size;
        public List<int> normalization_lookups;

        public Loader(int locus_offset, int loci_buffer_size, List<int> normalization_lookups)
        {
            /*
            Constructor

            Args:
                locus_offset(int): Start loading data at this locus index
                loci_buffer_size(int): Number of loci to load(starting at offset)
                normalization_lookups(list(int)): List of normalization lookups(for all loci in sample data, not just slice).

              Returns:
                    Loader: The new Loader object
            */
            this.locus_offset = locus_offset;
            this.loci_buffer_size = loci_buffer_size;
            this.normalization_lookups = normalization_lookups;
        }

        public LocusAggregate __call__(GenotypeCalls sample_data) {
            int locus_offset = this.locus_offset;
            int loci_buffer = this.loci_buffer_size;
            LocusAggregate locus_aggregate = new LocusAggregate();
            locus_aggregate.genotypes = new List<byte>(sample_data.get_genotypes(
                locus_offset, loci_buffer_size)).Cast<int>().ToList();
            locus_aggregate.scores = sample_data.get_genotype_scores(
                locus_offset, loci_buffer_size).Cast<float?>().ToList();
            if(sample_data.version >= 4) {
                locus_aggregate.b_allele_freqs = sample_data.get_ballele_freqs(locus_offset, loci_buffer_size).Cast<float?>().ToList();
                locus_aggregate.log_r_ratios = sample_data.get_logr_ratios(locus_offset, loci_buffer_size).Cast<float?>().ToList();
            }
            else {
                locus_aggregate.b_allele_freqs = Enumerable.Repeat<float?>(null, loci_buffer_size).ToList();
                locus_aggregate.log_r_ratios = Enumerable.Repeat<float?>(null, loci_buffer_size).ToList();
            }
            locus_aggregate.x_intensities = sample_data.get_raw_x_intensities(locus_offset, loci_buffer_size).Cast<float?>().ToList();
            locus_aggregate.y_intensities = sample_data.get_raw_y_intensities(locus_offset, loci_buffer_size).Cast<float?>().ToList();
            List<NormalizationTransform> transforms = sample_data.get_normalization_transforms();
            locus_aggregate.transforms = this.normalization_lookups.Skip(locus_offset).Take(loci_buffer_size).Select(x => transforms[x]).ToList();
            return (locus_aggregate);
        }
    }
}
