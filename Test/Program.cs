using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;

using GTCParse;
using Simulations;
using Plink;
using static S3IO.S3;
using static Plink.PlinkS3;

namespace Test
{
    class Program
    {

        static void Main(string[] args)
        {
            var plink = readPlinkS3(bucketRegion: "ap-northeast-1", bucketName: "s3plink", keyFAM: "ted.fam", keyBED: "ted.bed", keyBIM: null, keyBIMFile: "ted_bim.txt", bucketRegionBIM: "ap-northeast-1", bucketNameBIM: "s3bpm");
            Console.WriteLine(plink.beds.Count);
        }
    }
}
