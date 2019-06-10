using System;
using System.IO;
using System.Collections.Generic;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class NormalizationTransform
    {
        public int version;
        public float offset_x, offset_y, scale_x, scale_y, shear, theta;
        public NormalizationTransform(BinaryReader buffer)
        {
            this.version = (int) read_int(buffer);
            this.offset_x = (float) read_float(buffer);
            this.offset_y = (float) read_float(buffer);
            this.scale_x = (float) read_float(buffer);
            this.scale_y = (float) read_float(buffer);
            this.shear = (float) read_float(buffer);
            this.theta = (float) read_float(buffer);
        }
    }
}
