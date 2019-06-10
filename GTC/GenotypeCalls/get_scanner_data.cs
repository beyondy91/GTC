using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public ScannerData get_scanner_data()
        {
            /*
            Returns:
                ScannerData: Information about scanner
            */
            return ((ScannerData)this.__get_generic(GenotypeCalls.__ID_SCANNER_DATA, ScannerData.read_scanner_data));
        }
    }
}