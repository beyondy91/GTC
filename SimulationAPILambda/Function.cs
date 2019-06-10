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
using Simulations;
using DynamoIO;
using GTCParse;
using Preferences;
using static S3IO.S3;
using Progress;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SimulationAPILambda
{
    public class SimulationAPILambdaFunction
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

            Document writeDocument = new Document();

            string sn = input["sn"].ToString();
            string stage = String.Format("{0}-{1}-{2}", input["code"], input["type"], "simulation");

            writeDocument["sn"] = sn;
            writeDocument["code"] = String.Format("{0}-{1}", input["code"], input["type"]);
            writeDocument["status"] = @"start";

            Dynamo.writeDynamoTable(tableRegion: preference["Dynamo"]["Simulation"]["Region"].ToString(),
                tableName: preference["Dynamo"]["Simulation"]["Name"].ToString(),
                writeDocument: writeDocument).Wait();
            await ProgressWriter.writer(sn: sn, stage: stage, status: "start");

            List <SimulationMeta> metas = JArray.Parse(input["meta"].ToString()).Select(input_meta => 
                new SimulationMeta(input_meta as JObject)).ToList();

            context.Logger.LogLine("number of meta data: " + metas.Count);

            if (!input.ContainsKey("size"))
                input["size"] = 1000000;

            string body = "";

            context.Logger.LogLine("Simulation start");
            float[] results = Simulation.RunSimulation(metas, Convert.ToInt32(input["size"]));
            SimulationResult simulationResult = new SimulationResult(results);
            writeStreamFromStreamToS3(bucketRegion: preference["S3"]["Simulation"]["Region"].ToString(),
                keyName: String.Format("{0}-{1}-{2}.sim", input["sn"], input["code"], input["type"]),
                bucketName: preference["S3"]["Simulation"]["Name"].ToString(),
                stream: simulationResult.ToStream()).Wait();

            writeDocument["status"] = @"end";

            Dynamo.writeDynamoTable(tableRegion: preference["Dynamo"]["Simulation"]["Region"].ToString(),
                tableName: preference["Dynamo"]["Simulation"]["Name"].ToString(),
                writeDocument: writeDocument).Wait();
            await ProgressWriter.writer(sn: sn, stage: stage, status: "end");

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
