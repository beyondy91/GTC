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
        public static List<Document> readDynamoTable(string tableRegion,
            string tableName,
            Expression queryExpression = null,
            QueryFilter queryFilter = null,
            Primitive hashKey = null,
            string credential = @"embeded",
            AmazonDynamoDBClient dynamoDBClient = null)
        {
            AmazonDynamoDBClient client;
            if (dynamoDBClient != null)
                client = dynamoDBClient;
            else
            {
                AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
                // This client will access the US East 1 region.
                clientConfig.RegionEndpoint = RegionEndpoint.GetBySystemName(tableRegion);
                client = new AmazonDynamoDBClient(clientConfig);
            }

            Table table = Table.LoadTable(client, tableName);

            var result = new List<Document>();

            Search search;

            if(queryExpression == null && queryFilter == null)
            {
                search = table.Scan(new ScanFilter());
            }
            else if (queryFilter == null)
            {
                search = table.Query(hashKey, queryExpression);
            }
            else
            {
                search = table.Query(hashKey, queryFilter);
            }

            do
            {
                result.AddRange(search.GetNextSetAsync().Result);
            } while (!search.IsDone);

            return result;
        }
    }
}
