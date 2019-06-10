using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        BinaryReader generic_seeker(short toc_entry)
        {
            BinaryReader gtc_handle = new BinaryReader(this.gtc_stream); 
            gtc_handle.BaseStream.Seek(this.toc_table[toc_entry], SeekOrigin.Begin);
            return (gtc_handle);
        }
    }
}