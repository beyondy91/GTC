using System;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Preferences;
using static DynamoIO.Dynamo;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ListTraitAPILambda
{
    public partial class ListTraitAPILambdaFunction
    {
        async Task<APIGatewayProxyResponse> CreateGetResponse(JObject input, ILambdaContext context)
        {
            JObject preference = new Preference().preference;

            int statusCode = (input != null) ?
                (int)HttpStatusCode.OK :
                (int)HttpStatusCode.InternalServerError;

            List<Document> result = readDynamoTable(tableRegion: preference["Dynamo"]["Meta"]["Region"].ToString(),
                tableName: preference["Dynamo"]["Meta"]["Name"].ToString());

            var response = new APIGatewayProxyResponse
            {
                StatusCode = statusCode,
                Body = new JArray(result.Select(x => x.ToJson()).ToList()).ToString(),
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
