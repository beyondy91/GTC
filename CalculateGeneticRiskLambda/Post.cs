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
using Preferences;
using static S3IO.S3;
using static DynamoIO.Dynamo;
using CalculationResult;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace CalculateGeneticRiskLambda
{
    public partial class CalculateGeneticRiskLambdaFunction
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
    }
}
