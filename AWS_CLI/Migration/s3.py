# -*- coding: utf-8 -*-
from __future__ import print_function
import sys, os
sys.path.append(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))
os.chdir(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))

from Default.defaults import *
from S3.createS3Bucket import createS3Bucket
from S3.putBucketEncryption import putBucketEncryption
from S3.putBucketFullAccessPolicy import putBucketFullAccessPolicy

# GTC 저장용 S3 생성
createS3Bucket(client=clientS3,
               ACL='private',
               Bucket=bucketGTC,
               CreateBucketConfiguration={
                   'LocationConstraint': locationConstraint
               },
               ObjectLockEnabledForBucket=True)
putBucketEncryption(client=clientS3,
                    Bucket=bucketGTC,
                    SSEAlgorithm='AES256')

# BPM 저장용 S3 생성
createS3Bucket(client=clientS3,
               ACL='private',
               Bucket=bucketBPM,
               CreateBucketConfiguration={
                   'LocationConstraint': locationConstraint
               },
               ObjectLockEnabledForBucket=True)
putBucketEncryption(client=clientS3,
                    Bucket=bucketBPM,
                    SSEAlgorithm='AES256')

# Plink 저장용 S3 생성
createS3Bucket(client=clientS3,
               ACL='private',
               Bucket=bucketPlink,
               CreateBucketConfiguration={
                   'LocationConstraint': locationConstraint
               },
               ObjectLockEnabledForBucket=True)
putBucketEncryption(client=clientS3,
                    Bucket=bucketPlink,
                    SSEAlgorithm='AES256')

# chain 저장용 S3 생성
createS3Bucket(client=clientS3,
               ACL='private',
               Bucket=bucketChain,
               CreateBucketConfiguration={
                   'LocationConstraint': locationConstraint
               },
               ObjectLockEnabledForBucket=True)
putBucketEncryption(client=clientS3,
                    Bucket=bucketChain,
                    SSEAlgorithm='AES256')

# Simulation 결과 저장용 S3 생성
createS3Bucket(client=clientS3,
               ACL='private',
               Bucket=bucketSimulation,
               CreateBucketConfiguration={
                   'LocationConstraint': locationConstraint
               },
               ObjectLockEnabledForBucket=True)
putBucketEncryption(client=clientS3,
                    Bucket=bucketSimulation,
                    SSEAlgorithm='AES256')



# bucket policy 추가
putBucketFullAccessPolicy(resource=resourceS3,
                          BucketName='s3gtc',
                          RoleArn=arnRoleLambdaS3GTCToPlink,
                          PolicyFile='JSON/defaultBucketPolicy.json')
putBucketFullAccessPolicy(resource=resourceS3,
                          BucketName='s3bpm',
                          RoleArn=arnRoleLambdaS3GTCToPlink,
                          PolicyFile='JSON/defaultBucketPolicy.json')
putBucketFullAccessPolicy(resource=resourceS3,
                          BucketName='s3plink',
                          RoleArn=arnRoleLambdaS3GTCToPlink,
                          PolicyFile='JSON/defaultBucketPolicy.json')