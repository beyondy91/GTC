using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static DynamoIO.Dynamo;
using Preferences;

namespace CalculationResult
{
    public partial class Calculate
    {
        public static float calc(JArray metas,
            JObject answer)
        {
            float result = 0;
            foreach(JObject meta in metas)
            {
                if(answer.ContainsKey(meta["var"].ToString()) && answer[meta["var"].ToString()].ToString() == meta["meta"]["Eff"].ToString())
                {
                    result += Convert.ToSingle(meta["meta"]["beta"].ToString());
                }
            }
            return result;
        }
    }
}
