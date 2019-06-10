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
using BPMUtils;
using Amazon.Lambda.APIGatewayEvents;
using GTCParse;
using static S3IO.S3;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace BPMToBIMAPILambda
{
    public class BPMToBIMAPILambdaFunction
    {
        public APIGatewayProxyResponse Post(
                Stream stream, ILambdaContext context)
        {
            using (StreamReader sr = new StreamReader(stream))
            {
                string body;
                body = sr.ReadToEnd();
                context.Logger.LogLine("stream length: " + stream.Length.ToString());
                context.Logger.LogLine("body: " + body);
                var input = JObject.Parse(body);
                return CreateResponse(input).Result;
            }
        }

        async Task<APIGatewayProxyResponse> CreateResponse(JObject input)
        {
            JObject preference = new Preference().preference;

            int statusCode = (input != null) ?
                (int)HttpStatusCode.OK :
                (int)HttpStatusCode.InternalServerError;

            string bucketName = input.ContainsKey("bucketName") ? input["bucketName"].ToString() : preference["S3"]["BPM"]["Name"].ToString();
            string bucketRegion = input.ContainsKey("bucketRegion") ? input["bucketRegion"].ToString() : preference["S3"]["BPM"]["Region"].ToString();

            var bpm = new BeadPoolManifest(new BinaryReader(readStreamFromS3ToMemory(bucketName: bucketName,
                            keyName: input["BPMKey"].ToString(),
                            bucketRegion: bucketRegion).Result));
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);

            for (int idx = 0; idx < bpm.num_loci; ++idx)
            {
                List<string> bimEntry = new List<string> { bpm.chroms[idx], bpm.names[idx], "0", bpm.map_infos[idx].ToString() };
                List<string> alleles = bpm.ref_strands[idx] == 1 ? bpm.snps[idx].Replace("[", "").Replace("]", "").Split('/').ToList() :
                    bpm.snps[idx].Replace("[", "").Replace("]", "").Split('/').Select(x => Utils.getComplement(x[0]).ToString()).ToList();
                bimEntry.AddRange(alleles);
                sw.WriteLine(String.Join(' ', bimEntry));
                sw.Flush();
            }

            writeStreamFromStreamToS3(bucketName: bucketName,
                keyName: input["BPMKey"].ToString().Replace("bpm","bim"),
                bucketRegion: bucketRegion,
                stream: ms).Wait();

            string body = "";

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
