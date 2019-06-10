using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace GTCParse.BeadPoolManifestAmend
{
    public static partial class BeadPoolManifestAmend
    {
        /*
            Method for amending binary(BPM) manifest file.

            Attributes:
            bpm (BeadPoolManifest): original manifest file
            amend_file: amend info file (csv format, no header)
            amend (list({'from': SNP name in original manifest,
                        'to': SNP name in new manifest, 
                        'ref_strands': ref_strands in new manifest,
                        'snps': snps in new manifest}))
                       
            names (list(strings): Names of loci from manifest
            snps (list(strings): SNP values of loci from manifest

            chroms (list(strings): Chromosome values for loci
            map_infos (list(int)): Map infor values for loci
            addresses(list(int)): AddressA IDs of loci from manifest
            normalization_lookups(list(int)) : Normalization lookups from manifest.This indexes into
                                             list of normalization transforms read from GTC file
            ref_strands (list(int)): Reference strand annotation for loci(see RefStrand class)
            source_strands(list(int)) : Source strand annotations for loci(see SourceStrand class)
            num_loci(int) : Number of loci in manifest
            manifest_name(string): Name of manifest
            control_config(string): Control description from manifest
        */

        public static BeadPoolManifest Amend(BeadPoolManifest bpm, List<BeadPoolManifestAmendEntry> amendEntries)
        {
            for(int idx = 0; idx < bpm.num_loci; ++idx)
            {
                foreach(BeadPoolManifestAmendEntry amendEntry in amendEntries)
                {
                    if(amendEntry.originalSNPNameOnBPM == bpm.names[idx])
                    {
                        // Console.WriteLine("Index: " + idx + ", locus_name: " + bpm.locus_entries[idx].name + ", name: " + x["to"] + ", Strand: " + x["ref_strands"] + ", snps: " + x["snps"]);
                        bpm.names[idx] = amendEntry.newSNPNameOnBPM;
                        // bpm.ref_strands[idx] = Convert.ToInt32(x["ref_strands"]);
                        bpm.locus_entries[idx].ref_strand = amendEntry.newStrandOnBPM;
                        // bpm.snps[idx] = x["snps"];
                        bpm.locus_entries[idx].snp = amendEntry.newAlleleOnBPM;
                        bpm.locus_entries[idx].name = amendEntry.newSNPNameOnBPM;
                    }
                }
            }
            return bpm;
        }
    }
}
