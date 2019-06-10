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


namespace ListTraitAPILambda
{
    public partial class ListTraitAPILambdaFunction
    {
        public APIGatewayProxyResponse Get(
                Stream stream, ILambdaContext context)
        {
            using (StreamReader sr = new StreamReader(stream))
            {
                string body;
                body = sr.ReadToEnd();
                context.Logger.LogLine(body);
                var input = JObject.Parse(body);
                return CreateGetResponse(input, context).Result;
            }
        }
    }
}
