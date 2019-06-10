using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Preferences;
using GTCParse;

namespace S3IO
{
    public static partial class S3
    {
        public static async Task<Stream> readStreamFromS3ToMemory(string bucketName,
            string keyName,
            string bucketRegion,
            string credential=@"embeded",
            AmazonS3Client s3client = null)
        {
            AmazonS3Client client;
            if (credential != @"embeded")
            {
                client = new AmazonS3Client(RegionEndpoint.GetBySystemName(bucketRegion));
            }
            else if (s3client != null)
            {
                client = s3client;
            }
            else
            {
                client = new AmazonS3Client();
            }
            Console.WriteLine("Getting object starts");
            var request = new Amazon.S3.Model.GetObjectRequest
            {
                BucketName = bucketName,
                Key = keyName
            };
            Console.WriteLine("BucketName: " + bucketName);
            Console.WriteLine("Key: " + keyName);
            GetObjectResponse response = await client.GetObjectAsync(request);
            return response.ResponseStream;
        }
    }
}