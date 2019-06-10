using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda;
using Amazon.Lambda.Model;


namespace Simulations
{
    public class SurveySimulationGenerator
    {

        public async static Task generator(string sn,
            string code,
            string sex,
            List<SimulationMeta> metas,
            int size = 1000000,
            string bucketRegion = null,
            string bucketName = null,
            JObject preference = null)
        {
            List<string> keys = metas.Select(x => x.var).ToList();
            Dictionary<string, int> keyMap = new Dictionary<string, int>();
            for(int idx = 0; idx < metas.Count; ++idx)
            {
                keyMap[metas[idx].var] = idx;
            }
            string objectKey = code;
            keys.Sort();
            string apiKey = preference == null ? System.Environment.GetEnvironmentVariable("APIKey") : preference["API"]["InternalSimulationAPIFunction-API"]["key"].ToString();
            string apiUrl = preference == null ? System.Environment.GetEnvironmentVariable("APIUrl") : preference["API"]["InternalSimulationAPIFunction-API"]["url"].ToString();
            Console.WriteLine("API url: " + apiUrl);
            JObject apiBody = new JObject();
            apiBody["sn"] = sn;
            apiBody["type"] = "survey";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-API-Key", apiKey);
            client.DefaultRequestHeaders.Add("X-Amz-Invocation-Type", "Event");
            Console.WriteLine("http client ready");
            Console.WriteLine("number of meta: " + metas.Count);
            AmazonLambdaClient lambdaClient = new AmazonLambdaClient();

            /// the number of all possibilities of combination : 2 ^ (number of survey)
            for (ulong idx = 1; idx < (ulong)(1 << metas.Count); ++idx)
            {
                var combination = new byte[metas.Count];
                foreach(SimulationMeta meta in metas)
                {
                    var meta_idx = keyMap[meta.var];
                    combination[meta_idx] = (byte)((idx >> meta_idx) - (idx >> (meta_idx + 1) << 2));
                }
                var metaCombination = new JArray();
                var keyCombination = code + "_" + sex;
                for (int meta_idx = 0; meta_idx < metas.Count; ++meta_idx)
                {
                    if (combination[meta_idx] == (byte)1)
                    {
                        keyCombination += "_" + keys[meta_idx];
                        metaCombination.Add(metas[keyMap[keys[meta_idx]]].ToJObject());
                    }
                    apiBody["meta"] = metaCombination;
                }
                apiBody["code"] = keyCombination;
                Console.WriteLine(idx + "th post body ready");
                Console.WriteLine(apiBody.ToString());
                Console.WriteLine("invoking");
                try
                {
                    await client.PostAsync(apiUrl, new StringContent(apiBody.ToString(), Encoding.UTF8, "application/json"));
                    // await lambdaClient.InvokeAsync(lambdaRequest);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine(e.Data);
                }
                Console.WriteLine("invoked");
            }
        }
    }
}
