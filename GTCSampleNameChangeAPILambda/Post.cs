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
