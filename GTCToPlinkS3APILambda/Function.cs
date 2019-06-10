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
using GTCToPlink.S3;
using Preferences;
using Amazon.Lambda.APIGatewayEvents;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace GTCToPlinkS3APILambda
{
    public class GTCToPlinkS3APILambdaFunction
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
                return CreateResponse(input).Result;
            }
        }

        async Task<APIGatewayProxyResponse> CreateResponse(JObject input)
        {
            JObject preference = new Preference().preference;

            int statusCode = (input != null) ?
                (int)HttpStatusCode.OK :
                (int)HttpStatusCode.InternalServerError;

            var result = GTCToPlinkS3.ConvertGTCToPlinkS3(bucketNameGTC: input.ContainsKey("bucketNameGTC") ? input["bucketNameGTC"].ToString(): preference["S3"]["GTC"]["Name"].ToString(),
                    bucketNameBPM: input.ContainsKey("bucketNameBPM") ? input["bucketNameBPM"].ToString() : preference["S3"]["BPM"]["Name"].ToString(),
                    bucketNamePlink: input.ContainsKey("bucketNamePlink") ? input["bucketNamePlink"].ToString() : preference["S3"]["Plink"]["Name"].ToString(),
                    bucketRegionGTC: input.ContainsKey("bucketRegionGTC") ? input["bucketRegionGTC"].ToString() : preference["S3"]["GTC"]["Region"].ToString(),
                    bucketRegionBPM: input.ContainsKey("bucketRegionBPM") ? input["bucketRegionBPM"].ToString() : preference["S3"]["BPM"]["Region"].ToString(),
                    bucketRegionPlink: input.ContainsKey("bucketRegionPlink") ? input["bucketRegionPlink"].ToString() : preference["S3"]["Plink"]["Region"].ToString(),
                    keyNameGTCs: input["GTCKeys"].Select(x => x.ToString()).ToList(),
                    keyNamePlinkBasedir: input.ContainsKey("plinkBase") ? input["plinkBase"].ToString() : preference["PlinkBase"].ToString(),
                    tableRegion: input.ContainsKey("dynamoProgressRegion") ? input["dynamoProgressRegion"].ToString() : preference["Dynamo"]["Progress"]["Region"].ToString(),
                    tableName: input.ContainsKey("dynamoProgressTable") ? input["dynamoProgressTable"].ToString() : preference["Dynamo"]["Progress"]["Name"].ToString(),
                    credential: @"embeded");

            string body = String.Join(",", result);

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
