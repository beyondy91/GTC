using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Plink
{
    public partial class BIM
    {
        public string Chromosome, snpName, Allele1, Allele2;
        public int cM, Position;
        public BIM(string Chromosome, string snpName, int cM, int Position, string Allele1, string Allele2)
        {
            this.Chromosome = Chromosome;
            this.snpName = snpName;
            this.cM = cM;
            this.Position = Position;
            this.Allele1 = Allele1;
            this.Allele2 = Allele2;
        }
    }
}