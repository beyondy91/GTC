# -*- coding: utf-8 -*-
from __future__ import print_function
import boto3, json, base64, os, tempfile, zipfile, sys, codecs, uuid

vpcName = 'ds-pipeline-vpc'

privateSubnetName = 'json_default'

publicSubnetName = 'public'

privateRouteTableName = 'json_default'

publicRouteTableName = 'public'

securityGroupName = 'ds-pipeline-group'

clientEC2 = boto3.client('ec2')

clientIAM = boto3.client('iam')

clientS3 = boto3.client('s3')

resourceS3 = boto3.resource('s3')

clientDynamoDB = boto3.client('dynamodb')

clientDynamoDBStreams = boto3.client('dynamodbstreams')

resourceDynamoDB = boto3.resource('dynamodb')

clientLambda = boto3.client('lambda')

clientKMS = boto3.client('kms')

clientAPI = boto3.client('apigateway')

pathDefault = '/pipeline/'

locationConstraint = 'ap-northeast-1'

privateCidrBlock = '172.32.0.0/24'

publicCidrBlock = '172.32.1.0/24'

bucketGTC = 's3gtc'

bucketBPM = 's3bpm'

bucketPlink = 's3plink'

bucketChain = 's3chain'

bucketSimulation = 's3simulation'

tablePreference = 'g1preference'

tableProgress = 'g1progress'

tableFilename = 'g1filename'

tableManifest = 'g1manifest'

tableSimulation = 'g1simulation'

tableMetaGenotype = 'g1metagenotype'

tableMetaSurvey = 'g1metasurvey'

tableMeta = 'g1meta'

tableResult = 'g1result'

account = boto3.client('sts').get_caller_identity().get('Account')

rootArn = "arn:aws:iam::%s:root" % account

preferences = [
    {
        'key': 'S3',
        'preference': {
            'GTC': {
                'Region': locationConstraint,
                'Name': bucketGTC
            },
            'BPM': {
                'Region': locationConstraint,
                'Name': bucketBPM
            },
            'Plink': {
                'Region': locationConstraint,
                'Name': bucketPlink
            },
            'Simulation': {
                'Region': locationConstraint,
                'Name': bucketSimulation
            },
            'Chain': {
                'Region': locationConstraint,
                'Name': bucketChain
            }
        }
    },
    {
        'key': 'Dynamo',
        'preference': {
            'Progress': {
                'Region': locationConstraint,
                'Name': tableProgress
            },
            'Filename': {
                'Region': locationConstraint,
                'Name': tableFilename
            },
            'Simulation': {
                'Region': locationConstraint,
                'Name': tableSimulation
            },
            'Manifest': {
                'Region': locationConstraint,
                'Name': tableManifest
            },
            'MetaGenotype': {
                'Region': locationConstraint,
                'Name': tableMetaGenotype
            },
            'MetaSurvey': {
                'Region': locationConstraint,
                'Name': tableMetaSurvey
            },
            'Meta': {
                'Region': locationConstraint,
                'Name': tableMeta
            },
            'Result': {
                'Region': locationConstraint,
                'Name': tableResult
            }
        }
    },
    {
        'key': 'PlinkBase',
        'preference': ''
    }
]

DefaultPreference = 'tableRegionPreference="{0}";tableNamePreference="{1}";tableRegionManifest="{2}";tableNameManifest="{3}"'.format(locationConstraint, tablePreference, locationConstraint, tableManifest)

with open(os.path.join(os.path.dirname(os.path.abspath(__file__)), '../JSON/defaultAPIPolicy.json')) as inf:
    DefaultAPIPolicy = inf.read()