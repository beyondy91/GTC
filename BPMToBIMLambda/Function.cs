using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Preferences;
using Amazon.DynamoDBv2.DocumentModel;
using GTCParse;
using BPMUtils;
using Amazon.Lambda.APIGatewayEvents;
using static S3IO.S3;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace BPMToBIMLambda
{
    public class BPMToBIMLambdaFunction
    {
        IAmazonS3 S3Client { get; set; }

        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public BPMToBIMLambdaFunction()
        {
            AmazonS3Config config = new AmazonS3Config();
            config.ForcePathStyle = true;
            config.UseHttp = false;
            S3Client = new AmazonS3Client(config);
        }

        /// <summary>
        /// Constructs an instance with a preconfigured S3 client. This can be used for testing the outside of the Lambda environment.
        /// </summary>
        /// <param name="s3Client"></param>
        public BPMToBIMLambdaFunction(IAmazonS3 s3Client)
        {
            this.S3Client = s3Client;
        }
        
        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an S3 event object and can be used 
        /// to respond to S3 notifications.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task BPMToBIMLambdaFunctionHandler(S3Event evnt, ILambdaContext context)
        {
            JObject preference = new Preference().preference;

            var s3Event = evnt.Records?[0].S3;
            if(s3Event == null)
            {
                return;
            }

            try
            {
                var bpm = new BeadPoolManifest(new BinaryReader(readStreamFromS3ToMemory(bucketName: s3Event.Bucket.Name,
                                keyName: s3Event.Object.Key,
                                bucketRegion: preference["S3"]["BPM"]["Region"].ToString()).Result));
                var ms = new MemoryStream();
                var sw = new StreamWriter(ms);

                for(int idx = 0; idx < bpm.num_loci; ++idx)
                {
                    List<string> bimEntry = new List<string> { bpm.chroms[idx], bpm.names[idx], "0", bpm.map_infos[idx].ToString() };
                    List<string> alleles = bpm.ref_strands[idx] == 1 ? bpm.snps[idx].Replace("[", "").Replace("]", "").Split('/').ToList() :
                        bpm.snps[idx].Replace("[", "").Replace("]", "").Split('/').Select(x => Utils.getComplement(x[0]).ToString()).ToList();
                    bimEntry.AddRange(alleles);
                    sw.WriteLine(String.Join(' ', bimEntry));
                    sw.Flush();
                }

                writeStreamFromStreamToS3(bucketName: s3Event.Bucket.Name,
                    keyName: s3Event.Object.Key.Replace("bpm", "bim"),
                    bucketRegion: preference["S3"]["BPM"]["Region"].ToString(),
                    stream: ms).Wait();
            }

            catch(Exception e)
            {
                context.Logger.LogLine(e.Message);
                context.Logger.LogLine(e.StackTrace);
                throw;
            }
        }
    }
}
