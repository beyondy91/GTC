using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;


namespace GTCParse
{
    public partial class ClusterRecord
    {
        /*
        Store clustering information for a single locus

        Attributes:
            aa_cluster_stats(ClusterStats) : Describes AA genotype cluster
            ab_cluster_stats(ClusterStats) : Describes AB genotype cluster
            bb_cluster_stats(ClusterStats) : Describes BB genotype cluster
            intensity_threshold(float) : Intensity threshold for no-calll
            cluster_score(ClusterStore): Various scores for cluster
           address(int): Bead type identifier for probe A
       */

        public ClusterStats aa_cluster_stats, ab_cluster_stats, bb_cluster_stats;
        public float intensity_threshold;
        public ClusterScore cluster_score;
        public int? address;

        public ClusterRecord(ClusterStats aa_cluster_stats, ClusterStats ab_cluster_stats, ClusterStats bb_cluster_stats,
                             float intensity_threshold, ClusterScore cluster_score, int? address)
        {

            this.aa_cluster_stats = aa_cluster_stats;
            this.ab_cluster_stats = ab_cluster_stats;
            this.bb_cluster_stats = bb_cluster_stats;
            this.intensity_threshold = intensity_threshold;
            this.cluster_score = cluster_score;
            this.address = address;
        }
    }
}
