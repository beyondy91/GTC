using System;
using System.Collections.Generic;
using System.Linq;

namespace GTCParse
{
    public class LocusAggregate
    {


        /*
            Class to contain aggregated data for a single locus
            across many samples.For each attribute list, the individual lists
            are those values as a slice across the specified samples

            Attributes:
                genotypes (list(int)): List of integer genotypes
                scores(list(float)) : List of Gencall scores
                b_allele_freqs(list(float)) : List of b allele frequencies
                log_r_ratios(list(float)): List of log R ratios
               x_intensities(list(int)): List of raw x intensities
              y_intensities(list(int)): List of raw y intensities
             transforms(list(NormalizationTransform)): List of normalization transforms
        */
        public List<int> genotypes;
        public List<float?> scores, b_allele_freqs, log_r_ratios, x_intensities, y_intensities;
        public List<NormalizationTransform> transforms;
        public LocusAggregate()
        {
            this.genotypes = new List<int>();
            this.scores = new List<float?>();
            this.b_allele_freqs = new List<float?>();
            this.log_r_ratios = new List<float?>();
            this.x_intensities = new List<float?>();
            this.y_intensities = new List<float?>();
        }

        public static List<LocusAggregate> load_buffer(List<GenotypeCalls> samples, int locus_offset, int loci_buffer_size, List<int> normalization_lookups)
        {
            /*
                Load a subset of continguous loci across a number of samples

                Args:
                    samples(list(GenotypeCalls)): The samples to aggregate
                    locus_offset(int): Index of the first locus to load
                    loci_buffer_size(int): Total number of loci to load for each sample:
                    process_pool(multiprocessing.Pool): Process pool for running parallel operations


               Returns:

                   list(LocusAggregate): A locus aggregate object for each sample

            */

            return (samples.Select(x => new Loader(locus_offset, loci_buffer_size, normalization_lookups).__call__(x)).ToList());
        }

        public static IEnumerable<List<int>> group_loci(List<int> loci, int loci_batch_size)
        {
            /*
            Group loci indices such that the first and last loci
            are not separated by more than the batch size

            Args:
                loci(list(int)): List of loci indices(must be sorted)
                loci_batch_size(int): Maximum difference to allow between first and last grouped index

            Yields:
                list(int) : Grouped loci indexes
            */
            List<int> current_indices = new List<int>();
            foreach(int locus in loci) {
                if(current_indices.Count != 0) {
                    if(current_indices[current_indices.Count-1] > locus) {
                        throw new Exception("Loci indices are not sorted");
                    }
                    if (locus - loci_batch_size >= current_indices[0]) {
                        yield return (current_indices);
                        current_indices = new List<int>();
                    }
                }
                current_indices.Add(locus);
            }
            if (current_indices.Count > 0)
                yield return (current_indices);
        }

        public static IEnumerable<List<object>> aggregate_samples(List<GenotypeCalls> samples, IEnumerable<int> loci, Func<LocusAggregate, object> callback,
                                                     List<int> normalization_lookups, int bin_size= 100000000) {
            /*
                Generate LocusAggregate information from a collection of samples. Will call the callback
            function for a LocusAggregate object for each specified locus index and yield the result.

            Args:
                samples(list(GenotypeCalls)): The samples to aggregate for each locus

               loci(iter(int)): Enumerates the loci indices of interest(must be sorted in ascending order)

               callback(func): A function that takes a LocusAggregate and return a new result
                bin_size(int): Used to determine how much data will be loaded into memory at one time. Larger bin size will use more memory and(generally) run faster. This bin_size already accounts for how many samples are being handled.

          Yields:

              Result of callback function

            */
            // figure out how many loci to load at once
            int loci_batch_size = (int)(bin_size / (float)samples.Count) + 1;

            foreach(List<int> loci_group in LocusAggregate.group_loci(loci.ToList(), loci_batch_size)) {
                // read in the buffer for this group of loci
                List<LocusAggregate> buffer = LocusAggregate.load_buffer(
                    samples, loci_group[0], loci_group[-1] - loci_group[0] + 1, normalization_lookups);

                // generate corresponding locus aggregates
                List<LocusAggregate> aggregates = loci_group.Select(x => new GenerateLocusAggregate(buffer, loci_group[0]).__call__(x)).ToList();

                foreach (List<object> result in aggregates.Select(x => callback(x)).ToList())
                    yield return (result);
            }


        }
    }
}
