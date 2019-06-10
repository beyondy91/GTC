using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GTCToPlink.S3;
using Preferences;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.APIGatewayEvents;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace GTCToPlinkS3Lambda
{
    public class GTCToPlinkS3LambdaFunction
    {
        IAmazonS3 S3Client { get; set; }

        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public GTCToPlinkS3LambdaFunction()
        {
            AmazonS3Config config = new AmazonS3Config();
            config.ForcePathStyle = true;
            config.UseHttp = false;
            S3Client = new AmazonS3Client(config);
        }

        /// <summary>
        /// Constructs an instance with a preconfigured S3 client. This can be used for testing the outside of the Lambda environment.
        /// </summary>
        /// <param name="s3Client"></param>
        public GTCToPlinkS3LambdaFunction(IAmazonS3 s3Client)
        {
            this.S3Client = s3Client;
        }
        
        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an S3 event object and can be used 
        /// to respond to S3 notifications.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task GTCToPlinkS3LambdaFunctionHandler(S3Event evnt, ILambdaContext context)
        {
            JObject preference = new Preference().preference;

            var s3Event = evnt.Records?[0].S3;
            if(s3Event == null)
            {
                return;
            }

            try
            {
                var result = GTCToPlinkS3.ConvertGTCToPlinkS3(bucketNameGTC: s3Event.Bucket.Name,
                    bucketNameBPM: preference["S3"]["BPM"]["Name"].ToString(),
                    bucketNamePlink: preference["S3"]["Plink"]["Name"].ToString(),
                    bucketRegionGTC: preference["S3"]["GTC"]["Name"].ToString(),
                    bucketRegionBPM: preference["S3"]["BPM"]["Region"].ToString(),
                    bucketRegionPlink: preference["S3"]["Plink"]["Region"].ToString(),
                    keyNameGTCs: new List<string> { s3Event.Object.Key },
                    keyNamePlinkBasedir: preference["PlinkBase"].ToString(),
                    tableRegion: preference["Dynamo"]["Progress"]["Region"].ToString(),
                    tableName: preference["Dynamo"]["Progress"]["Name"].ToString(),
                    credential: @"embeded",
                    s3client: (AmazonS3Client) this.S3Client);
            }
            catch(Exception e)
            {
                context.Logger.LogLine(e.Message);
                context.Logger.LogLine(e.StackTrace);
                throw;
            }
        }
    }
}
