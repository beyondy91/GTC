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
using static DynamoIO.Dynamo;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Simulations
{
    public class SurveySimulationAPILambdaFunction
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
                return CreatePostResponse(input, context).Result;
            }
        }

        async Task<APIGatewayProxyResponse> CreatePostResponse(JObject input, ILambdaContext context)
        {
            JObject preference = new Preference().preference;

            int statusCode = (input != null) ?
                (int)HttpStatusCode.OK :
                (int)HttpStatusCode.InternalServerError;

            context.Logger.LogLine("written document");

            context.Logger.LogLine(input["meta"].ToString());

            List <SimulationMeta> metas = (JArray.Parse(input["meta"].ToString())).Select(input_meta => 
                new SimulationMeta(input_meta as JObject)).ToList();

            if (!input.ContainsKey("size"))
                input["size"] = 1000000;


            await SurveySimulationGenerator.generator(sn: "@cache",
                code: input["code"].ToString(),
                sex: input["sex"].ToString(),
                metas: metas,
                size: Convert.ToInt32(input["size"]),
                bucketRegion: input.ContainsKey("bucketRegion")? input["bucketRegion"].ToString() : null,
                bucketName: input.ContainsKey("bucketName") ? input["bucketName"].ToString() : null,
                preference: preference);

            var response = new APIGatewayProxyResponse
            {
                StatusCode = statusCode,
                Body = "",
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
