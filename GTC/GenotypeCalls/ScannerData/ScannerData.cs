using System;
using System.IO;

namespace GTCParse
{
    public partial class ScannerData
    {
        public string name;
        public int pmt_green;
        public int pmt_red;
        public string version;
        public string user;

        public ScannerData(string name, int pmt_green, int pmt_red, string version, string user)
        {
            /*
                    Constructor

                    Args:
                        name(string): scanner identifier
                        pmt_green(int): gain setting(green channel)
                        pmt_red(int): gain setting(red channel)
                        version(string): version of scanner software
                        user(string): user of the scanner software

                   Returns:
                        ScannerData
            */
            this.name = name;
            this.pmt_green = pmt_green;
            this.pmt_red = pmt_red;
            this.version = version;
            this.user = user;
        }
    }
}
