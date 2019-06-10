using System;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.S3Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Amazon.Lambda.APIGatewayEvents;
using DynamoIO;
using Preferences;
using static S3IO.S3;
using static DynamoIO.Dynamo;
using CalculationResult;

namespace CalculateGeneticRiskLambda
{
    public partial class CalculateGeneticRiskLambdaFunction
    {
        public async Task S3Handler(S3Event evnt, ILambdaContext context)
        {
            JObject preference = new Preference().preference;

            var s3Event = evnt.Records?[0].S3;
            if (s3Event == null)
            {
                return;
            }

            try
            {
                var input = new JObject();
                input["sn"] = s3Event.Object.Key.Replace(".bed", "");
                await CreatePostResponse(input: input, context: context);
            }
            catch (Exception e)
            {
                context.Logger.LogLine(e.Message);
                context.Logger.LogLine(e.StackTrace);
                throw;
            }
        }
    }
}
