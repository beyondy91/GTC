using System;
using System.IO;
using System.Collections.Generic;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class NormalizationTransform
    {
        public float?[] normalize_intensities(int x, int y, bool threshold = true)
        {
            /*
            Apply this normalization transform to raw intensities

            Args:
                x (int): Raw x intensities
                y (int): Raw y intensities

            Returns:
                float[]: (xn, yn) normalized intensities as tuple of floats
            */
            float?[] result = new float?[2];
            if (x <= 0 && y <= 0)
            {
                result[0] = null;
                result[1] = null;
                return (result);
            }

            float tempx = x - this.offset_x;
            float tempy = y - this.offset_y;

            float tempx2 = (float)(Math.Cos(this.theta) * tempx + Math.Sin(this.theta) * tempy);
            float tempy2 = (float)(-Math.Sin(this.theta) * tempx + Math.Cos(this.theta) * tempy);

            float tempx3 = tempx2 - this.shear * tempy2;
            float tempy3 = tempy2;

            float xn = tempx3 / this.scale_x;
            float yn = tempy3 / this.scale_y;

            if (threshold)
            {
                xn = Math.Max(0, xn);
                yn = Math.Max(0, yn);
            }
            result[0] = xn;
            result[1] = yn;
            return (result);
        }
    }
}
