using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using static S3IO.S3;

namespace Plink
{
    public class PlinkS3
    {
        public static Plink readPlinkS3(string bucketRegion,
            string bucketName,
            string keyFAM,
            string keyBED,
            string keyBIM = null,
            string bucketRegionBIM = null,
            string bucketNameBIM = null,
            string keyBIMFile = null)
        {
            if (keyBIM == null)
            {
                using (var sr = new StreamReader(readStreamFromS3ToMemory(bucketRegion: bucketRegion,
                    bucketName: bucketName,
                    keyName: keyBIMFile).Result))
                {
                    keyBIM = sr.ReadToEnd();
                }
            }
            Console.WriteLine("KeyBIM: " + keyBIM);
            if (bucketRegionBIM == null)
            {
                bucketRegionBIM = bucketRegion;
            }
            if (bucketNameBIM == null)
            {
                bucketNameBIM = bucketName;
            }

            return Plink.readPlinkFromStream(sr_fam: new StreamReader(readStreamFromS3ToMemory(bucketRegion: bucketRegion,
                        bucketName: bucketName,
                        keyName: keyFAM).Result),
                    sr_bim: new StreamReader(readStreamFromS3ToMemory(bucketRegion: bucketRegionBIM,
                        bucketName: bucketNameBIM,
                        keyName: keyBIM).Result),
                    br_bed: new BinaryReader(readStreamFromS3ToMemory(bucketRegion: bucketRegion,
                        bucketName: bucketName,
                        keyName: keyBED).Result));
        }
    }
}
