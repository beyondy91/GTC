using System;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


using Amazon.Lambda.Core;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.DynamoDBEvents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Amazon.Lambda.APIGatewayEvents;
using DynamoIO;
using Preferences;
using static S3IO.S3;
using static DynamoIO.Dynamo;
using CalculationResult;
using Meta;
using Simulations;
using Progress;

namespace CalculateGeneticRiskLambda
{
    public partial class CalculateGeneticRiskLambdaFunction
    {
        public async Task PostSimulationHandler(DynamoDBEvent evnt, ILambdaContext context)
        {
            JObject preference = new Preference().preference;

            var dynamoDBEvent = evnt.Records[0]?.Dynamodb;
            if (dynamoDBEvent == null)
            {
                return;
            }

            try
            {
                string tableRegionResult = preference["Dynamo"]["Result"]["Region"].ToString();

                string tableNameResult = preference["Dynamo"]["Result"]["Name"].ToString();

                string tableRegionSimulation = preference["Dynamo"]["Simulation"]["Region"].ToString();

                string tableNameSimulation = preference["Dynamo"]["Simulation"]["Name"].ToString();

                var entry = dynamoDBEvent.Keys;

                var sn = entry["sn"].S;
                var code = entry["code"].S;

                if (Regex.IsMatch(code, "-genotype$"))
                {
                    var progressDoc = readDynamoTable(tableRegion: tableRegionSimulation,
                        tableName: tableNameSimulation,
                        hashKey: sn,
                        queryFilter: new QueryFilter()).Where(doc => doc["code"].ToString() == code).ToArray()[0];
                    if (progressDoc["status"].ToString() == "end")
                    {
                        string stage = String.Format("{0}-{1}-{2}", code, "genotype", "postsimulation");
                        await ProgressWriter.writer(sn: sn, stage: stage, status: "start");
                        Document resultDoc = readDynamoTable(tableRegion: tableRegionResult,
                            tableName: tableNameResult,
                            hashKey: sn,
                            queryFilter: new QueryFilter()).Where(doc => doc["code"].ToString() == Regex.Replace(code, "-genotype$", "")).ToArray()[0];
                        JObject resultJobj = JObject.Parse(resultDoc["result"].ToString());

                        JToken resultGenotype = resultJobj["genotype"];
                        float risk = Convert.ToSingle(resultGenotype["risk"].ToString());
                        SimulationResult resultSim = new SimulationResult(new BinaryReader(readStreamFromS3ToMemory(bucketRegion: preference["S3"]["Simulation"]["Region"].ToString(),
                            bucketName: preference["S3"]["Simulation"]["Name"].ToString(),
                            keyName: String.Format("{0}-{1}-{2}.sim", sn, Regex.Replace(code, "-genotype$", ""), "genotype")).Result));
                        resultGenotype["times"] = Math.Exp(risk - resultSim.mean).ToString();
                        resultGenotype["rank"] = (resultSim.result.Where(x => x > risk).ToList().Count * 1.0 / resultSim.size).ToString();

                        resultJobj["genotype"] = resultGenotype;

                        resultDoc["result"] = resultJobj.ToString();

                        await writeDynamoTable(tableRegion: tableRegionResult,
                            tableName: tableNameResult,
                            writeDocument: resultDoc);
                        await ProgressWriter.writer(sn: sn, stage: stage, status: "end");
                    }
                }

            }
            catch (Exception e)
            {
                context.Logger.LogLine(e.Message);
                context.Logger.LogLine(e.StackTrace);
            }
        }
    }
}