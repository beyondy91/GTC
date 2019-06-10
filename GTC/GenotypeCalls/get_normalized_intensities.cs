using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public List<float?[]> get_normalized_intensities(List<int> normalization_lookups)
        {
            /*
            Calculate and return the normalized intensities

            Args:
                normalization_lookups(list(int)) : Map from each SNP to a normalization transform.
                                       This list can be obtained from the BeadPoolManifest object.

            Return:
                list((float?, float?)): The list of the normalized intensities for the sample as a list of (x, y) float tuples
            */
            List<float?[]> result = new List<float?[]>();
            List<NormalizationTransform> normalization_transforms = this.get_normalization_transforms();
            List<int> raw_x_intensities = this.get_raw_x_intensities(), raw_y_intensities = this.get_raw_y_intensities();
            for (int i = 0; i < normalization_lookups.Count; ++i)
            {
                if (i < raw_x_intensities.Count)
                {
                    int lookup = normalization_lookups[i];
                    if (lookup < normalization_transforms.Count)
                    {
                        int x_raw = raw_x_intensities[i];
                        int y_raw = raw_y_intensities[i];
                        result.Add(normalization_transforms[lookup].normalize_intensities(x_raw, y_raw));
                    }
                    else
                    {
                        result.Add(new float?[] { null, null });
                    }
                }
            }
            return (result);
        }
    }
}