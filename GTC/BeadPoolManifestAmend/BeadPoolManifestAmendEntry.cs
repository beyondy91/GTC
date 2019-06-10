using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace GTCParse.BeadPoolManifestAmend
{
    public class BeadPoolManifestAmendEntry
    {
        public string originalSNPNameOnBPM, newSNPNameOnBPM;
        public int newStrandOnBPM;
        public string newAlleleOnBPM;
        public BeadPoolManifestAmendEntry(string originalSNPNameOnBPM, string newSNPNameOnBPM, int newStrandOnBPM, string newAlleleOnBPM)
        {
            this.originalSNPNameOnBPM = originalSNPNameOnBPM;
            this.newSNPNameOnBPM = newSNPNameOnBPM;
            this.newStrandOnBPM = newStrandOnBPM;
            this.newAlleleOnBPM = newAlleleOnBPM;
        }
    }
}