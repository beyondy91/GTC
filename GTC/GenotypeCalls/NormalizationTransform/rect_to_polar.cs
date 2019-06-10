using System;
using System.IO;
using System.Collections.Generic;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class NormalizationTransform
    {
        public static float?[] rect_to_polar(float?[] intensity)
        {
            /*
            Converts normalized x,y intensities to(pseudo) polar co-ordinates(R, theta)

            Args:
                intensity(float[]) : Normalized x, y intensities for probe

             Returns:
                float[]: (R, theta) polar values as tuple of floats
            */
            float x = Convert.ToSingle(intensity[0]);
            float y = Convert.ToSingle(intensity[1]);
            float?[] result = new float?[2];
            if (Math.Abs(x) < float.Epsilon && Math.Abs(y) < float.Epsilon)
            {
                result[0] = null;
                result[1] = null;
            }
            else
            {
                result[0] = x + y;
                result[1] = (float)(Math.Atan2(y, x) * 2.0 / Math.PI);
            }
            return (result);
        }
    }
}