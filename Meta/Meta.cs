using System;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DynamoIO;
using static DynamoIO.Dynamo;

namespace Meta
{
    public class AllMeta
    {
        public List<Document> metas;
        public HashSet<string> codes;

        public AllMeta(string tableRegion, string tableName)
        {
            this.codes = new HashSet<string>();

            this.metas = readDynamoTable(tableRegion: tableRegion,
                tableName: tableName);

            foreach (Document meta in this.metas)
            {
                this.codes.Add(meta["code"].ToString());
            }
        }

        public JArray MetasByCode(string code)
        {
            List<JObject> result = new List<JObject>();

            foreach(Document meta in this.metas)
            {
                if(meta["code"].ToString() == code)
                {
                    var metaJobject = JObject.Parse(meta["meta"].AsDocument().ToJson());
                    metaJobject["var"] = meta["var"].ToString();
                    result.Add(metaJobject);
                }
            }

            return new JArray(result);
        }
    }
}
