using System;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Preferences;
using GTCParse;
using static S3IO.S3;

namespace GTCSampleNameChangeLambda
{
    public partial class GTCSampleNameChangeLambdaFunction
    {
        async Task<APIGatewayProxyResponse> CreatePostResponse(JObject input, ILambdaContext context)
        {
            JObject preference = new Preference().preference;

            int statusCode = (input != null) ?
                (int)HttpStatusCode.OK :
                (int)HttpStatusCode.InternalServerError;

            string fileName = input["filename_origin"].ToString();
            string sn = input["sn"].ToString();
            string newFileName = input["filename_new"].ToString();
            context.Logger.LogLine(String.Format("change sample name in {0} to {1} and filename to {2}", fileName, sn, newFileName));

            GenotypeCalls gtc = new GenotypeCalls(new BinaryReader(readStreamFromS3ToMemory(bucketName: preference["S3"]["GTC"]["Name"].ToString(),
                bucketRegion: preference["S3"]["GTC"]["Region"].ToString(),
                keyName: fileName).Result));

            gtc.change_sample_name(sn);

            gtc.gtc_stream.Seek(0, SeekOrigin.Begin);

            string body = "Failed: New filename already exists";

            try
            {
                readStreamFromS3ToMemory(bucketName: preference["S3"]["GTC"]["Name"].ToString(),
                bucketRegion: preference["S3"]["GTC"]["Region"].ToString(),
                keyName: newFileName).Wait();
            }
            catch
            {
                await writeStreamFromStreamToS3(bucketName: preference["S3"]["GTC"]["Name"].ToString(),
                    bucketRegion: preference["S3"]["GTC"]["Region"].ToString(),
                    keyName: newFileName,
                    stream: gtc.gtc_stream);
                body = "success";
            }

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
