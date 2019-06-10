using System;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Preferences;
using Meta;

namespace GenotypeSimulationLambda
{
    public partial class GenotypeSimulationLambdaFunction
    {
        async Task<APIGatewayProxyResponse> CreatePostResponse(JObject input, ILambdaContext context)
        {
            JObject preference = new Preference().preference;

            int statusCode = (input != null) ?
                (int)HttpStatusCode.OK :
                (int)HttpStatusCode.InternalServerError;

            string sn = input["sn"].ToString();

            AllMeta allMeta = new AllMeta(tableRegion: preference["Dynamo"]["MetaGenotype"]["Region"].ToString(),
                tableName: preference["Dynamo"]["MetaGenotype"]["Name"].ToString());

            HashSet<string> codes = input.ContainsKey("codes") ? ((JArray)input["codes"]).Select(x => x.ToString()).ToHashSet() : allMeta.codes;

            string apiKey = preference == null ? System.Environment.GetEnvironmentVariable("APIKey") : preference["API"]["InternalCalcGenoRiskAPIFunction-API"]["key"].ToString();
            string apiUrl = preference == null ? System.Environment.GetEnvironmentVariable("APIUrl") : preference["API"]["InternalCalcGenoRiskAPIFunction-API"]["url"].ToString();
            JObject apiBody = new JObject();
            apiBody["sn"] = sn;
            apiBody["codes"] = new JArray(codes);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-API-Key", apiKey);
            client.DefaultRequestHeaders.Add("X-Amz-Invocation-Type", "Event");

            await client.PostAsync(apiUrl, new StringContent(apiBody.ToString()));

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
