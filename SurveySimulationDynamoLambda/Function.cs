using System;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.DynamoDBEvents;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Preferences;
using static DynamoIO.Dynamo;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SurveySimDynLambda
{
    public class SurveySimDynLambdaFunction
    {
        IAmazonDynamoDB DynamoDBClient { get; set; }

        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public SurveySimDynLambdaFunction()
        {
            AmazonDynamoDBConfig config = new AmazonDynamoDBConfig();
            DynamoDBClient = new AmazonDynamoDBClient(config);
        }

        /// <summary>
        /// Constructs an instance with a preconfigured S3 client. This can be used for testing the outside of the Lambda environment.
        /// </summary>
        /// <param name="s3Client"></param>
        public SurveySimDynLambdaFunction(IAmazonDynamoDB DynamoDBClient)
        {
            this.DynamoDBClient = DynamoDBClient;
        }
        
        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an S3 event object and can be used 
        /// to respond to S3 notifications.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task SurveySimDynLambdaFunctionHandler(DynamoDBEvent evnt, ILambdaContext context)
        {
            JObject preference = new Preference().preference;

            var dynamoDBEvent = evnt.Records[0]?.Dynamodb;
            if(dynamoDBEvent == null)
            {
                return;
            }

            try
            {
                var entry = dynamoDBEvent.Keys;

                JObject apiBody = new JObject();
                apiBody["code"] = entry["code"].S;

                await CreatePostResponse(input: apiBody, context: context);
            }
            catch(Exception e)
            {
                context.Logger.LogLine(e.Message);
                context.Logger.LogLine(e.StackTrace);
            }
        }



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

            string apiKey = preference == null ? System.Environment.GetEnvironmentVariable("APIKey") : preference["API"]["InternalSurveySimAPIFunction-API"]["key"].ToString();
            string apiUrl = preference == null ? System.Environment.GetEnvironmentVariable("APIUrl") : preference["API"]["InternalSurveySimAPIFunction-API"]["url"].ToString();

            JObject apiBody = new JObject();
            apiBody["code"] = input["code"].ToString();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-API-Key", apiKey);
            client.DefaultRequestHeaders.Add("X-Amz-Invocation-Type", "Event");

            List<Document> metaDocs = readDynamoTable(tableRegion: preference["Dynamo"]["MetaSurvey"]["Region"].ToString(),
                    tableName: preference["Dynamo"]["MetaSurvey"]["Name"].ToString(),
                    queryFilter: new QueryFilter(),
                    hashKey: input["code"].ToString()
                );
            context.Logger.LogLine("code: " + apiBody["code"]);
            context.Logger.LogLine("Number of meta: " + metaDocs.Count);
            apiBody["meta"] = new JArray();
            foreach (var metaDoc in metaDocs)
            {
                var metas = JArray.Parse(metaDoc["meta"].ToString());
                foreach (string sex in new List<string> { "M", "F" })
                {
                    apiBody["sex"] = sex;
                    apiBody["meta"] = new JArray(apiBody["meta"].Concat(metas.Where(meta => meta["sex"].ToString() == sex)));
                }
                context.Logger.LogLine("body: " + apiBody["meta"]);

                await client.PostAsync(apiUrl, new StringContent(apiBody.ToString()));
            }


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
