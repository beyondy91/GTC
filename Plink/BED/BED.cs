using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Plink
{
    public class BED
    {
        public List<byte> Genotype;
        public BED(List<byte> Genotype)
        {
            this.Genotype = Genotype;
        }
    }
}