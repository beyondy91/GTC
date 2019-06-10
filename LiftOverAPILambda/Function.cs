using System;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Amazon.Lambda.APIGatewayEvents;
using DynamoIO;
using GTCParse;
using Preferences;
using static S3IO.S3;
using GenomeChain;
using static GenomeChain.LiftOver;
using Plink;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LiftOverAPILambda
{
    public class LiftOverAPILambdaFunction
    {
        public APIGatewayProxyResponse Post(
                Stream stream, ILambdaContext context)
        {
            using (StreamReader sr = new StreamReader(stream))
            {
                string body;
                body = sr.ReadToEnd();
                context.Logger.LogLine(body);
                var input = JObject.Parse(body);
                return CreateResponse(input, context).Result;
            }
        }

        async Task<APIGatewayProxyResponse> CreateResponse(JObject input, ILambdaContext context)
        {
            JObject preference = new Preference().preference;

            int statusCode = (input != null) ?
                (int)HttpStatusCode.OK :
                (int)HttpStatusCode.InternalServerError;

            string body = "";

            string chainBucketRegion = input.ContainsKey("chainBucketRegion") ? input["chainBucketRegion"].ToString() : preference["S3"]["Chain"]["Region"].ToString();
            string chainBucketName = input.ContainsKey("chainBucketName") ? input["chainBucketName"].ToString() : preference["S3"]["Chain"]["Name"].ToString();
            string chainKey = input["chainKey"].ToString();

            string bimBucketRegion = input.ContainsKey("bimBucketRegion") ? input["bimBucketRegion"].ToString() : preference["S3"]["BPM"]["Region"].ToString();
            string bimBucketName = input.ContainsKey("bimBucketName") ? input["bimBucketName"].ToString() : preference["S3"]["BPM"]["Name"].ToString();
            string bimKey = input["bimKey"].ToString();
            string bimKeyNew = input["bimKeyNew"].ToString();

            context.Logger.LogLine("chain file laoding");
            Chain chain;
            using (Stream stream = readStreamFromS3ToMemory(bucketRegion: chainBucketRegion,
                bucketName: chainBucketName,
                keyName: chainKey).Result)
            using (GZipStream gs = new GZipStream(stream, CompressionMode.Decompress))
            {
                chain = new Chain(stream: gs);
            }
            context.Logger.LogLine("chain file laoded");


            context.Logger.LogLine("bim file laoding");
            List<BIM> bims;
            using (Stream stream = readStreamFromS3ToMemory(bucketRegion: bimBucketRegion,
                bucketName: bimBucketName,
                keyName: bimKey).Result)
            using (StreamReader sr = new StreamReader(stream))
            {
                bims = Plink.Plink.readBIMFromStream(sr).AsParallel().Select((bim, bim_idx) => {
                    return bimLiftOver(chain: chain, input: bim);
                }).ToList();
            }
            context.Logger.LogLine("bim file laoded");

            context.Logger.LogLine("bim file writing");
            using (MemoryStreamNotDispose ms = new MemoryStreamNotDispose())
            using (StreamWriter sw = new StreamWriter(ms))
            {
                Plink.Plink.writeBIMToStream(sw_bim: sw, bims: bims);
                writeStreamFromStreamToS3(bucketRegion: bimBucketRegion,
                    bucketName: bimBucketName,
                    keyName: bimKeyNew,
                    stream: ms).Wait();
            }
            context.Logger.LogLine("bim file written");

            var response = new APIGatewayProxyResponse
            {
                StatusCode = statusCode,
                Body = body,
                Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/json" },
                        { "Access-Control-Allow-Origin", "*" }
                    }
            };

            return response;
        }
    }
}
