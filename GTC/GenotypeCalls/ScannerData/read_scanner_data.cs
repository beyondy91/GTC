using System;
using System.IO;

namespace GTCParse
{
    public partial class ScannerData
    {
        public static ScannerData read_scanner_data(BinaryReader handle)
        {
            /*
            Helper function to parse ScannerData object from file handle.

            Args:
                handle (BinaryReader): File handle

            Returns:
                ScannerData
            */
            string name = BeadArrayUtility.read_string(handle);
            int pmt_green = (int)BeadArrayUtility.read_int(handle);
            int pmt_red = (int)BeadArrayUtility.read_int(handle);
            string scanner_version = BeadArrayUtility.read_string(handle);
            string imaging_user = BeadArrayUtility.read_string(handle);
            return (new ScannerData(name, pmt_green, pmt_red, scanner_version, imaging_user));
        }
    }
}
