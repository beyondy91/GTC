using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Text.RegularExpressions;
using Amazon.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Preferences
{
    public partial class Preference
    {
        public JObject preference;
        public JArray bpm_mapping;

        public Preference(AmazonDynamoDBClient dynamoDBClient = null)
        {
            this.preference = new JObject();
            this.bpm_mapping = new JArray();

            AmazonDynamoDBClient clientPreference;
            if (dynamoDBClient != null)
                clientPreference = dynamoDBClient;
            else
            {
                AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
                clientConfig.RegionEndpoint = RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable("tableRegionPreference"));
                clientPreference = new AmazonDynamoDBClient(clientConfig);
            }

            AmazonDynamoDBClient clientManifest;
            if (dynamoDBClient != null)
                clientManifest = dynamoDBClient;
            else
            {
                AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
                clientConfig.RegionEndpoint = RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable("tableRegionManifest"));
                clientManifest = new AmazonDynamoDBClient(clientConfig);
            }
            Table preferenceTable = Table.LoadTable(clientPreference, Environment.GetEnvironmentVariable("tableNamePreference"));
            Table bpmTable = Table.LoadTable(clientManifest, Environment.GetEnvironmentVariable("tableNameManifest"));

            ScanFilter scanFilter = new ScanFilter();

            Search search = preferenceTable.Scan(scanFilter);

            List<Document> documentList = new List<Document>();
            do
            {
                documentList = search.GetNextSetAsync().Result;
                foreach (var document in documentList)
                {
                    try
                    {
                        this.preference[document["key"]] = JObject.Parse(document["preference"].ToString());
                    }
                    catch
                    {
                        this.preference[document["key"]] = document["preference"].ToString();
                    }
                }
            } while (!search.IsDone);

            scanFilter = new ScanFilter();

            search = bpmTable.Scan(scanFilter);

            do
            {
                documentList = search.GetNextSetAsync().Result;
                foreach (var document in documentList)
                {
                    this.bpm_mapping.Add(JObject.Parse(document.ToJson()));
                }
            } while (!search.IsDone);
        }
    }
}
