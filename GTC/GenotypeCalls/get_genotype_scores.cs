using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public List<float> get_genotype_scores(int offset = 0, int? count = null)
        {
            /*
            Returns:
                list(float) : The genotype scores
            */
            return (this.__get_generic_array_numpy(GenotypeCalls.__ID_GENOTYPE_SCORES, sizeof(float), offset, count).Select(x => BitConverter.ToSingle(x, 0)).ToList());
        }
    }
}