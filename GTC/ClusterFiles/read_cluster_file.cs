using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace GTCParse
{
    public partial class ClusterFile
    {
        public static ClusterFile read_cluster_file(BinaryReader handle)
        {
            /*
            Read a cluster file

            Args:
                file: EGT cluster file handle

            Returns:
                ClusterFile

            Raises:
                Exception: Incompatible cluster file format
            */
            int version = (int)BeadArrayUtility.read_int(handle);
            if (version != 3)
                throw new Exception("Cluster file version " + version + " not supported");

            string gencall_version = BeadArrayUtility.read_string(handle);
            string cluster_version = BeadArrayUtility.read_string(handle);
            string call_version = BeadArrayUtility.read_string(handle);
            string normalization_version = BeadArrayUtility.read_string(handle);
            string date_created = BeadArrayUtility.read_string(handle);

            bool is_wgt = ((int)BeadArrayUtility.read_byte(handle) == 1);
            if (is_wgt == false)
                throw new Exception("Only WGT cluster file version supported");

            string manifest_name = BeadArrayUtility.read_string(handle);

            ClusterFile result = new ClusterFile(gencall_version, cluster_version, call_version,
                                                 normalization_version, date_created, manifest_name);

            int data_block_version = (int)BeadArrayUtility.read_int(handle);
            if (!(new List<int>(new int[] { 8, 9 }).Contains(data_block_version)))
                throw new Exception("Data block version in cluster file " + data_block_version + " not  supported");

            // opa
            string _ = BeadArrayUtility.read_string(handle);

            int num_records = (int)BeadArrayUtility.read_int(handle);
            List<ClusterRecord> cluster_records = ClusterFile.read_array(
                handle, num_records, (BinaryReader h) => ClusterRecord.read_record(h, data_block_version)).Cast<ClusterRecord>().ToList();
            List<ClusterScore> cluster_scores = ClusterFile.read_array(
                    handle, num_records, ClusterScore.read_record).Cast<ClusterScore>().ToList();

            // genotypes
            List<string> _string = ClusterFile.read_array(handle, num_records, BeadArrayUtility.read_string).Cast<string>().ToList();

            List<string> loci_names = ClusterFile.read_array(handle, num_records, BeadArrayUtility.read_string).Cast<string>().ToList();
            List<int> addresses = ClusterFile.read_array(handle, num_records, BeadArrayUtility.read_int).Cast<int>().ToList();

            // cluster counts
            List<List<int>> cluster_counts = new List<List<int>>();
            for (int idx = 0; idx < num_records; ++idx)
            {
                //3 corresponds to number genotypes (AA, AB, BB)
                cluster_counts.Add(ClusterFile.read_array(handle, 3, BeadArrayUtility.read_int).Cast<int>().ToList());
            }

            for (int idx = 0; idx < cluster_records.Count; ++idx)
            {
                ClusterRecord cluster_record = cluster_records[idx];
                List<int> count_record = cluster_counts[idx];
                System.Diagnostics.Debug.Assert(cluster_record.aa_cluster_stats.N == count_record[0]);
                System.Diagnostics.Debug.Assert(cluster_record.ab_cluster_stats.N == count_record[1]);
                System.Diagnostics.Debug.Assert(cluster_record.bb_cluster_stats.N == count_record[2]);
            }

            for (int idx = 0; idx < loci_names.Count; ++idx)
            {
                string locus_name = loci_names[idx];
                int address = addresses[idx];
                ClusterRecord cluster_record = cluster_records[idx];
                ClusterScore cluster_score = cluster_scores[idx];
                cluster_record.address = address;
                cluster_record.cluster_score = cluster_score;
                result.add_record(locus_name, cluster_record);
            }

            return (result);
        }

    }
}
