using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace S3IO
{
    public static partial class S3
    {
        public static async Task writeStreamFromStreamToS3(string bucketName,
            string keyName,
            string bucketRegion,
            Stream stream,
            string credential = @"embeded",
            AmazonS3Client s3client = null)
        {
            AmazonS3Client client;
            if (credential != @"embeded")
            {
                client = new AmazonS3Client(RegionEndpoint.GetBySystemName(bucketRegion));
            }
            else if(s3client != null)
            {
                client = s3client;
            }
            else
            {
                client = new AmazonS3Client();
            }
            // MemoryStream upload_stream = new MemoryStream();
            // stream.CopyTo(upload_stream);
            var fileTransferUtility =
                    new TransferUtility(client);
            // stream.Seek(0, SeekOrigin.Begin);

            Console.WriteLine("Writing Region: " + bucketRegion);
            Console.WriteLine("Writing Bucket: " + bucketName);
            Console.WriteLine("Writing Key: " + keyName);

            await fileTransferUtility.UploadAsync(stream,
                bucketName,
                keyName);
        }
    }
}