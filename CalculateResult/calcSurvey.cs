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
        public static CalcResult calcSurvey(string code,
            JObject answer,
            string tableRegion = null,
            string tableName = null)
        {
            var preference = new Preference().preference;
            JArray metas = new JArray();
            foreach(var meta in readDynamoTable(tableRegion: tableRegion?? preference["Dynamo"]["MetaSurvey"]["Region"].ToString(),
                tableName: tableName?? preference["Dynamo"]["MetaSurvey"]["Name"].ToString(),
                hashKey: code,
                queryFilter: new QueryFilter()).Select(x => JObject.Parse(x.ToJson())))
            {
                metas.Add(meta);
            }
            float result = calc(metas: metas,
                answer: answer);
            metas = new JArray(metas.Where(meta => answer.ContainsKey(meta["var"].ToString())));
            return new CalcResult(result, metas);
        }
    }
}
