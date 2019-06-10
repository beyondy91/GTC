using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public bool is_write_complete()
        {
            /*
            Check for last item written to GTC file to verify that write
            has successfully completed

            Args:
                None

            Returns:
                bool: Whether or not write is complete
            */
            if (this.version == 3)
            {
                try
                {
                    this.get_num_intensity_only();
                    return (true);
                }
                catch
                {
                    return (false);
                }
            }
            else if (this.version == 4)
            {
                try
                {
                    this.get_logr_ratios();
                    return (true);
                }
                catch
                {
                    return (false);
                }
            }
            else if (this.version == 5)
            {
                try
                {
                    this.get_slide_identifier();
                    return (true);
                }
                catch
                {
                    return (false);
                }
            }
            else
            {
                throw new Exception("Unable to test for write completion on version " + this.version + " GTC file");
            }
        }

    }
}
