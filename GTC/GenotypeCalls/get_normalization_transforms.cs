using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public List<NormalizationTransform> get_normalization_transforms()
        {
            /*
            Returns:
                List(NormalizationTransform) : The normalization transforms used during genotyping 
            */
            return (this.__get_generic_array(GenotypeCalls.__ID_NORMALIZATION_TRANSFORMS, NormalizationTransform.read_normalization_transform, 52, 0, null).Cast<NormalizationTransform>().ToList());
        }
    }
}