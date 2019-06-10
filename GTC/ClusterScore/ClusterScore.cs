using System;
using System.IO;
namespace GTCParse
{
    public partial class ClusterScore
    {
        /*
            All scores for a given locus clustering.

            Attributes:
                cluster_separation (float): A score measure the separation between genotype clusters
                total_score (float): The GenTrain score
                original_score (float): The original score before editing this cluster
                edited (bool): Whether this cluster has been manually manipulated
        */
        public float cluster_separation, total_score, original_score;
        public bool edited;

        public ClusterScore(float cluster_separation, float total_score, float original_score, bool edited)
        {
            /*
            Constructor

            Args:
                cluster_separation(float): A score measure the separation between genotype clusters
                total_score(float): The GenTrain score
                original_score(float): The original score before editing this cluster
                edited(bool): Whether this cluster has been manually manipulated

           Returns:
                ClusterScore
            */
            this.cluster_separation = cluster_separation;
            this.total_score = total_score;
            this.original_score = original_score;
            this.edited = edited;
        }
    }
}
