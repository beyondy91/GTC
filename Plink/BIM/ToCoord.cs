using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Plink
{
    public partial class BIM
    {
        public Coord ToCoord()
        {
            return new Coord(this.Chromosome, this.Position);
        }
    }
}