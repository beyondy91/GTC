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
    public struct CalcResult
    {
        public float result { get; set; }
        public JArray metas { get; set; }
        public CalcResult(float result, JArray metas)
        {
            this.result = result;
            this.metas = metas;
        }
    }
}
