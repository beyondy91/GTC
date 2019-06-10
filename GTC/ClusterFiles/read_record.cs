using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;


namespace GTCParse
{
    public partial class ClusterRecord
    {
        public static ClusterRecord read_record(BinaryReader handle, int version)
        {
            /*
            Read a cluster record from the file handle

            Args:
                handle(file): The file handle
                version(int): The cluster record version(from header)

            Returns:
            ClusterRecord: Result will not be populated with either address or scores(read separately)

            Raises:
            Exception: Unsupported cluster record version
            */
            List<int> _ = ClusterFile.read_array(handle, 3, BeadArrayUtility.read_int).Cast<int>().ToList();
            int aa_n = _[0], ab_n = _[1], bb_n = _[2];
            List<float> __ = ClusterFile.read_array(handle, 3, BeadArrayUtility.read_float).Cast<float>().ToList();
            float aa_r_dev = __[0], ab_r_dev = __[1], bb_r_dev = __[2];
            __ = ClusterFile.read_array(handle, 3, BeadArrayUtility.read_float).Cast<float>().ToList();
            float aa_r_mean = __[0], ab_r_mean = __[1], bb_r_mean = __[2];
            __ = ClusterFile.read_array(handle, 3, BeadArrayUtility.read_float).Cast<float>().ToList();
            float aa_theta_dev = __[0], ab_theta_dev = __[1], bb_theta_dev = __[2];
            __ = ClusterFile.read_array(handle, 3, BeadArrayUtility.read_float).Cast<float>().ToList();
            float aa_theta_mean = __[0], ab_theta_mean = __[1], bb_theta_mean = __[2];

            float intensity_threshold;
            float _float;
            if (version == 9)
                intensity_threshold = (float)BeadArrayUtility.read_float(handle);
            else if (version == 8)
            {
                _float = (float)BeadArrayUtility.read_float(handle);
                intensity_threshold = 0;
            }
            else
            {
                throw new Exception("Unsupported cluster record version " + version);
            }


            // read through unused fields
            for (int idx = 0; idx < 14; ++idx)
                _float = (float)BeadArrayUtility.read_float(handle);


            ClusterStats aa_cluster_stats = new ClusterStats(
                    aa_theta_mean, aa_theta_dev, aa_r_mean, aa_r_dev, aa_n);
            ClusterStats ab_cluster_stats = new ClusterStats(
                    ab_theta_mean, ab_theta_dev, ab_r_mean, ab_r_dev, ab_n);
            ClusterStats bb_cluster_stats = new ClusterStats(
                    bb_theta_mean, bb_theta_dev, bb_r_mean, bb_r_dev, bb_n);

            return (new ClusterRecord(aa_cluster_stats, ab_cluster_stats, bb_cluster_stats, intensity_threshold, null, null));
        }
    }
}
