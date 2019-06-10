using System;
using System.IO;
namespace GTCParse
{
    public partial class ClusterScore
    {
        public static ClusterScore read_record(BinaryReader handle)
        {
            /*
            Read a ClusterScore from a file handle

            Args:
                handle(file) : The file handle

            Returns:
                ClusterScore
            */
            float cluster_separation = (float)BeadArrayUtility.read_float(handle);
            float total_score = (float)BeadArrayUtility.read_float(handle);
            float original_score = (float)BeadArrayUtility.read_float(handle);
            bool edited = (((int)BeadArrayUtility.read_byte(handle)) != 0);
            return (new ClusterScore(cluster_separation, total_score, original_score, edited));
        }
    }
}
