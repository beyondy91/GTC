using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Plink
{
    public struct Coord
    {
        string chr { get; set; }
        int pos { get; set; }
        public Coord(string chr, int pos)
        {
            this.chr = chr;
            this.pos = pos;
        }
    }
}