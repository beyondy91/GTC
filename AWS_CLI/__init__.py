# -*- coding: utf-8 -*-
from __future__ import print_function

from Default.defaults import *
import IAM
import IAM.createRole 
import IAM.createPolicy 
import IAM.attachPolicies
import Network
import Network.createVPCEndpoint
import Network.createVPC
import Network.modifyVPCAttribute
import Network.createRouteTable
import Network.createSecurityGroup
import Network.createSubnet
import S3
import S3.createBucketNotification 
import S3.createS3Bucket 
import S3.getBucketArn 
import S3.putBucketFullAccessPolicy 
import S3.putBucketEncryption
import Lambda
import Lambda.addLambdaInvokePermission 
import Lambda.createLambdaFunction 
import Lambda.createLambdaMapping 
import Lambda.deployLambdaFunction 
import Lambda.getLambdaFunctionArn 
import Lambda.updateLambdaFunction
import DynamoDB
import DynamoDB.createDynamoTable
import KMS
import KMS.createKMSKey