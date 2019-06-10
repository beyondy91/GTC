using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace GTCParse
{
    public partial class ClusterFile
    {
        /*
        Represents an EGT cluster file

        Attributes:
            gencall_version(string) : The GenCall version
            cluster_version(string): The clustering algorithm version
            call_version(string) : The genotyping algorithm version
            normalization_version(string) : The normalization algorithm version
            date_created(string) : The date the cluster file was created(e.g., 3/9/2017 2:18:30 PM)
            manifest_name(string) : The manifest name used to build this cluster file
        */

        public string gencall_version, cluster_version, call_version, normalization_version, date_created, manifest_name;
        public Dictionary<string, ClusterRecord> name2cluster_record;

        public ClusterFile(string gencall_version, string cluster_version, string call_version, 
                           string normalization_version, string date_created, string manifest_name)
        {

            this.gencall_version = gencall_version;
            this.cluster_version = cluster_version;
            this.call_version = call_version;
            this.normalization_version = normalization_version;
            this.date_created = date_created;
            this.manifest_name = manifest_name;
            this.name2cluster_record = new Dictionary<string, ClusterRecord>();
        }
                                                        
    }
}
