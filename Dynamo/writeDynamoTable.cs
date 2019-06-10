using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace DynamoIO
{
    public static partial class Dynamo
    {
        public async static Task<Document> writeDynamoTable(string tableRegion,
            string tableName,
            Document writeDocument,
            string credential = @"embeded",
            AmazonDynamoDBClient dynamoDBClient = null)
        {
            Console.WriteLine(tableName + ", " + writeDocument.ToJson());

            AmazonDynamoDBClient client;
            if (dynamoDBClient != null)
            {
                Console.WriteLine("Setting Client from Input");
                client = dynamoDBClient;
            }
            else
            {
                AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
                clientConfig.RegionEndpoint = RegionEndpoint.GetBySystemName(tableRegion);
                Console.WriteLine("Region set to " + tableRegion);
                client = new AmazonDynamoDBClient(clientConfig);
                Console.WriteLine("Setting Client to " + client.ToString());
            }

            Table table = Table.LoadTable(client, tableName);
            Console.WriteLine(tableName + " table loaded");

            return await table.PutItemAsync(writeDocument);
        }
    }
}
