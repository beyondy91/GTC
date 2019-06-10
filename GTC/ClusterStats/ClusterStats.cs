using System;
namespace GTCParse
{
    public class ClusterStats
    {
        /*
        Represents statistics for a single genotype cluster.

        Attributes:
            theta_mean (float): Theta mean value
            theta_dev (float): Theta std devation value
            r_mean (float): R (intensity) mean value
            r_dev (float): R (intensity) std deviation value
            N (int): Number of samples assigned to cluster during training
        */

        public float theta_mean, theta_dev, r_mean, r_dev;
        public int N;
        public ClusterStats(float theta_mean, float theta_dev, float r_mean, float r_dev, int N)
        {
            /*
            Constructor

            Args:
                theta_mean(float): Theta mean value
                theta_dev(float): Theta std devation value
                r_mean(float): R(intensity) mean value
                r_dev(float): R(intensity) std deviation value
                N(int): Number of samples assigned to cluster during training

            Returns:
                ClusterStats
            */

            this.theta_mean = theta_mean;
            this.theta_dev = theta_dev;
            this.r_mean = r_mean;
            this.r_dev = r_dev;
            this.N = N;
        }
    }
}
