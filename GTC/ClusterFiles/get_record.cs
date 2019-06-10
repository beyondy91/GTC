using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace GTCParse
{
    public partial class ClusterFile
    {

        public ClusterRecord get_record(string name)
        {
            /*
            Get the record for a locus

            Args:
                name(string) : Locus name

            Returns:
                LocusRecord: The record associated with the locus

            Raises:
                KeyError: Locus name not present in cluster file
            */
            return (this.name2cluster_record[name]);
        }
    }
}