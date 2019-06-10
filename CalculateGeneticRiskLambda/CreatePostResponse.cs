using System;
using System.Net;
using System.Net.Http;
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
using static Plink.PlinkS3;
using CalculationResult;
using Meta;
using Simulations;
using Progress;

namespace CalculateGeneticRiskLambda
{
    public partial class CalculateGeneticRiskLambdaFunction
    {
        async Task<APIGatewayProxyResponse> CreatePostResponse(JObject input, ILambdaContext context)
        {
            JObject preference = new Preference().preference;

            int statusCode = (input != null) ?
                (int)HttpStatusCode.OK :
                (int)HttpStatusCode.InternalServerError;

            string sn = input["sn"].ToString();

            string tableRegionResult = input.ContainsKey("tableRegionResult") ?
                input["tableRegionResult"].ToString() :
                preference["Dynamo"]["Result"]["Region"].ToString();

            string tableNameResult = input.ContainsKey("tableNameResult") ?
                input["tableNameResult"].ToString() :
                preference["Dynamo"]["Result"]["Name"].ToString();

            AllMeta allMeta = new AllMeta(tableRegion: preference["Dynamo"]["MetaGenotype"]["Region"].ToString(),
                tableName: preference["Dynamo"]["MetaGenotype"]["Name"].ToString());

            HashSet<string> codes = input.ContainsKey("codes") ? JArray.Parse(input["codes"].ToString()).Select(code => code.ToString()).ToHashSet() : allMeta.codes;

            Plink.Plink plink = readPlinkS3(bucketRegion: preference["S3"]["Plink"]["Region"].ToString(),
                bucketName: preference["S3"]["Plink"]["Name"].ToString(),
                keyFAM: sn + ".fam",
                keyBED: sn + ".bed",
                keyBIMFile: sn + "_bim.txt",
                bucketRegionBIM: preference["S3"]["BPM"]["Region"].ToString(),
                bucketNameBIM: preference["S3"]["BPM"]["Name"].ToString());

            foreach (string code in codes)
            {
                var result = Calculate.calcGenotype(code: code,
                    sn: sn,
                    plink: plink);
                List<Document> resultDocs = readDynamoTable(tableRegion: tableRegionResult,
                    tableName: tableNameResult,
                    hashKey: sn,
                    queryFilter: new QueryFilter()).Where(doc => doc["code"].ToString() == code).ToList();
                Document resultDoc = resultDocs.Count > 0 ? resultDocs[0] : new Document();
                JObject resultJobj = resultDoc.ContainsKey("result") ? JObject.Parse(resultDoc["result"].ToString()) : new JObject();
                resultDoc["sn"] = sn;
                resultDoc["code"] = code;

                JObject resultGenotype = new JObject();
                resultGenotype["risk"] = result.result.ToString();

                resultJobj["genotype"] = resultGenotype;

                resultDoc["result"] = resultJobj.ToString();

                await writeDynamoTable(tableRegion: tableRegionResult,
                    tableName: tableNameResult,
                    writeDocument: resultDoc);


                string apiKey = preference == null ? System.Environment.GetEnvironmentVariable("APIKey") : preference["API"]["InternalSimulationAPIFunction-API"]["key"].ToString();
                string apiUrl = preference == null ? System.Environment.GetEnvironmentVariable("APIUrl") : preference["API"]["InternalSimulationAPIFunction-API"]["url"].ToString();

                JObject apiBody = new JObject();
                apiBody["sn"] = sn;
                apiBody["code"] = code;
                apiBody["type"] = "genotype";
                apiBody["meta"] = new JArray(result.metas.Select(meta =>
                {
                    JToken jt = meta["meta"];
                    jt["var"] = meta["var"];
                    return (jt);
                }).ToList());
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-API-Key", apiKey);
                client.DefaultRequestHeaders.Add("X-Amz-Invocation-Type", "Event");

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
