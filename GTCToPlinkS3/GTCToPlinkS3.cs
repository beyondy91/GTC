using Amazon;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;
using static S3IO.S3;
using Amazon.S3;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Amazon.DynamoDBv2.DocumentModel;
using static DynamoIO.Dynamo;
using Preferences;
using GTCParse;
using Progress;

namespace GTCToPlink.S3
{
    public static class GTCToPlinkS3
    {
        public static List<string> ConvertGTCToPlinkS3(string bucketNameGTC,
            List<string> keyNameGTCs,
            string bucketRegionGTC,
            string bucketNameBPM,
            string bucketRegionBPM,
            string bucketNamePlink,
            string bucketRegionPlink,
            string keyNamePlinkBasedir,
            string tableRegion,
            string tableName,
            string credential = @"embeded",
            AmazonS3Client s3client = null)
        {
            Console.WriteLine("Conversion function invoked");
            return keyNameGTCs.Select((keyNameGTC, gtc_idx) =>
            {
                string gtc_filename = keyNameGTC.Split('/').Last();
                List<GenotypeCalls> gtcs = new List<GenotypeCalls>
                {
                    new GenotypeCalls(new BinaryReader(readStreamFromS3ToMemory(bucketName: bucketNameGTC, keyName: keyNameGTC, bucketRegion: bucketRegionGTC, credential: credential, s3client: s3client).Result))
                };
                string plink_filename_base = keyNamePlinkBasedir != "\"\"" ? Path.Combine(keyNamePlinkBasedir, gtcs[0].get_sample_name()) : gtcs[0].get_sample_name();//Regex.Replace(gtc_filename, "\\.gtc$", ""));
                Console.WriteLine("sample name: " + gtcs[0].get_sample_name());

                ProgressWriter.writer(sn: gtcs[0].get_sample_name(), stage: "Plink_conversion", status: "start").Wait();

                try
                {
                    string bpm_in_gtc = gtcs[0].get_snp_manifest();
                    var bpm_mapping = new Preference().bpm_mapping;
                    for(int mapping_idx = 0; mapping_idx < bpm_mapping.Count; ++mapping_idx)
                    {
                        if(bpm_in_gtc == bpm_mapping[mapping_idx]["bpm"].ToString())
                        {
                            Plink.Plink plink = GTCToPlink.ConvertGTCToPlink(gtcs: gtcs, bpm: null);
                            plink.bimFile = bpm_mapping[mapping_idx]["manifest"].ToString() + ".bim";
                            Console.WriteLine("Plink conversion completed");
                            // var sw_bim_stream = new GZipStream(new MemoryStreamNotDispose(), CompressionMode.Compress);
                            // var sw_bim_stream = new MemoryStream();
                            var sw_bim_file_stream = new MemoryStreamNotDispose();
                            var sw_fam_stream = new MemoryStreamNotDispose();
                            var bw_bed_stream = new MemoryStreamNotDispose();
                            // var bw_strand_stream = new MemoryStream();
                            plink.writePlinkToStream(sw_bim: null,
                                sw_fam: new StreamWriter(sw_fam_stream),
                                bw_bed: new BinaryWriter(bw_bed_stream),
                                sw_bim_file: new StreamWriter(sw_bim_file_stream),
                                bw_strand: null);
                            writeStreamFromStreamToS3(bucketName: bucketNamePlink, keyName: plink_filename_base + ".bed", bucketRegion: bucketRegionPlink, stream: bw_bed_stream, credential: credential, s3client: s3client).Wait();
                            // writeStreamFromStreamToS3(bucketName: bucketNamePlink, keyName: plink_filename_base + ".bim.gz", bucketRegion: bucketRegionPlink, stream: sw_bim_stream.BaseStream, credential: credential, s3client: s3client).Wait();
                            writeStreamFromStreamToS3(bucketName: bucketNamePlink, keyName: plink_filename_base + "_bim.txt", bucketRegion: bucketRegionBPM, stream: sw_bim_file_stream, credential: credential, s3client: s3client).Wait();
                            writeStreamFromStreamToS3(bucketName: bucketNamePlink, keyName: plink_filename_base + ".fam", bucketRegion: bucketRegionPlink, stream: sw_fam_stream, credential: credential, s3client: s3client).Wait();
                            // writeStreamFromStreamToS3(bucketName: bucketNamePlink, keyName: plink_filename_base + ".strand", bucketRegion: bucketRegionPlink, stream: bw_strand_stream, credential: credential, s3client: s3client).Wait();

                            ProgressWriter.writer(sn: gtcs[0].get_sample_name(), stage: "Plink_conversion", status: "end").Wait();
                            return plink_filename_base;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    ProgressWriter.writer(sn: gtcs[0].get_sample_name(), stage: "Plink_conversion", status: "failed").Wait();
                }
                return null;
            }).ToList();
        }
    }
}