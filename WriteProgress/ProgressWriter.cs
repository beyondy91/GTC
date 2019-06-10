using Amazon;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;
using Amazon.S3;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Amazon.DynamoDBv2.DocumentModel;
using static DynamoIO.Dynamo;
using Preferences;

namespace Progress
{
    public class ProgressWriter
    {
        public async static Task writer(string sn, string stage, string status)
        {
            JObject preference = new Preference().preference;
            Document progressDocument = new Document();
            progressDocument["sn"] = sn;
            progressDocument["stage"] = stage;
            progressDocument["status"] = status;
            string tableRegion = preference["Dynamo"]["Progress"]["Region"].ToString();
            string tableName = preference["Dynamo"]["Progress"]["Name"].ToString();
            await writeDynamoTable(tableRegion: tableRegion, tableName: tableName, writeDocument: progressDocument);
        }

        public async static Task writer(Document progressDocument)
        {
            JObject preference = new Preference().preference;
            string tableRegion = preference["Dynamo"]["Progress"]["Region"].ToString();
            string tableName = preference["Dynamo"]["Progress"]["Name"].ToString();
            await writeDynamoTable(tableRegion: tableRegion, tableName: tableName, writeDocument: progressDocument);
        }
    }
}
