using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public List<int> get_control_x_intensities()
        {
            /*
            Returns:
                list(int) : The x intensities of control bead types
            */
            return (this.__get_generic_array_numpy(GenotypeCalls.__ID_CONTROLS_X, sizeof(ushort)).Select(x => Convert.ToInt32(BitConverter.ToUInt16(x, 0))).ToList());
        }
    }
}