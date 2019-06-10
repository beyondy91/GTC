using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Plink
{
    public partial class Plink
    {
        public List<FAM> fams;
        public List<BIM> bims;
        public string bimFile;
        public List<BED> beds;
        public int num_snps;
        public int byte_per_block;
        public List<byte[]> genotypes;
        public List<byte> strands;
        public Dictionary<string, int> snpMap;
        public Dictionary<string, Dictionary<int, List<int>>> coordMap;
        public Plink(List<string> FIDs,
            List<string> IIDs,
            List<string> Fathers,
            List<string> Mothers,
            List<byte> Sexs,
            List<byte> Phenotypes,
            List<byte[]> genotypes,
            List<string> Chromosomes = null,
            List<string> snpNames = null,
            List<int> cMs = null,
            List<int> Positions = null,
            List<List<string>> alleles = null,
            List<byte> strands = null,
            string bimFile=".bim")
        {
            this.genotypes = genotypes;
            if(strands != null)
            {
                this.strands = strands;
            }
            else
            {
                this.strands = new List<byte>();
            }
            int num_samples = FIDs.Count;
            this.num_snps = genotypes.Count;
            this.byte_per_block = (num_samples + 3) / 4;
            this.fams = Enumerable.Range(0, num_samples).AsParallel().Select(sample_idx =>
            {
                return new FAM(FID: FIDs[sample_idx], 
                    IID: IIDs[sample_idx], 
                    Father: Fathers[sample_idx], 
                    Mother: Mothers[sample_idx], 
                    Sex: Sexs[sample_idx], 
                    Phenotype: Phenotypes[sample_idx]);
            }).ToList();
            if(Chromosomes != null)
            {
                this.bims = Enumerable.Range(0, this.num_snps).AsParallel().Select(snp_idx =>
                {
                    return new BIM(Chromosome: Chromosomes[snp_idx],
                        snpName: snpNames[snp_idx],
                        cM: cMs[snp_idx],
                        Position: Positions[snp_idx],
                        Allele1: alleles[snp_idx][0],
                        Allele2: alleles[snp_idx][1]);
                }).ToList();
            }
            else
            {
                this.bims = new List<BIM>();
            }
        }
    }
}