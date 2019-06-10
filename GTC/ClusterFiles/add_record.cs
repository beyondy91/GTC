using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace GTCParse
{
    public partial class ClusterFile
    {
        public void add_record(string name, ClusterRecord record)
        {
            /*
            Add a new record to the cluster file

            Args:
                name(string) : Locus name
                record(ClusterRecord) : Record for the locus
            */
            this.name2cluster_record[name] = record;
        }
    }
}