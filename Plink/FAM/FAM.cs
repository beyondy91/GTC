using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Plink
{
    public class FAM
    {
        public string FID, IID, Father, Mother;
        public byte Sex, Phenotype;
        public int num_samples;
        public FAM(string FID, string IID, string Father="0", string Mother="0", byte Sex=0, byte Phenotype=1)
        {
            this.FID = FID;
            this.IID = IID;
            this.Father = Father;
            this.Mother = Mother;
            this.Sex = Sex;
            this.Phenotype = Phenotype;
        }
    }
}