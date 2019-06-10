using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public class MemoryStreamNotDispose : MemoryStream
    {
        public override void Close() { }
    }
}