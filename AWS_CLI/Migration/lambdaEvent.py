# -*- coding: utf-8 -*-
from __future__ import print_function
import sys, os

sys.path.append(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))
os.chdir(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))

from Default.defaults import *

from loadArnPolicy import *
from loadArnRole import *
from Lambda.deployLambdaEventDriven import deployLambdaEventDriven
from loadPrivateSubnet import *
from loadSecurityGroup import *
from S3.getBucketArn import getBucketArn
from DynamoDB.getTableArn import getTableArn

deployLambdaEventDriven(client=clientLambda,
                        Role=arnRoles['LambdaS3GTCToPlink'],
                        dir='../../GTCToPlinkS3Lambda',
                        SecurityGroupIds=groupId,
                        SubnetIds=privateSubnetId,
                        env=DefaultPreference,
                        deployConfig='aws-lambda-tools-defaults.json',
                        functionName='GTCToPlinkFunction',
                        sourceName=bucketGTC,
                        sourceService='s3',
                        sourceArn=getBucketArn(bucketGTC),
                        FilterRules=[
                            {
                                'Name': 'suffix',
                                'Value': '.gtc'
                            }
                        ])



deployLambdaEventDriven(client=clientLambda,
                        Role=arnRoles['LambdaS3BPMToBIM'],
                        dir='../../BPMToBIMLambda',
                        SecurityGroupIds=groupId,
                        SubnetIds=privateSubnetId,
                        env=DefaultPreference,
                        deployConfig='aws-lambda-tools-defaults.json',
                        functionName='BPMToBIMFunction',
                        sourceName=bucketBPM,
                        sourceService='s3',
                        sourceArn=getBucketArn(bucketBPM),
                        FilterRules=[
                            {
                                'Name': 'suffix',
                                'Value': '.bpm'
                            }
                        ])



deployLambdaEventDriven(client=clientLambda,
                        Role=arnRoles['LambdaCalcGenoRisk'],
                        dir='../../SurveySimulationDynamoLambda',
                        SecurityGroupIds=groupId,
                        SubnetIds=privateSubnetId,
                        env=DefaultPreference,
                        deployConfig='aws-lambda-tools-defaults.json',
                        functionName='SurveySimulationDynamoFunction',
                        sourceName=tableMetaSurvey,
                        sourceService='dynamodb',
                        sourceArn=getTableArn(Table=tableMetaSurvey))


deployLambdaEventDriven(client=clientLambda,
                        Role=arnRoles['LambdaCalcGenoRisk'],
                        dir='../../CalculateGeneticRiskLambda',
                        SecurityGroupIds=groupId,
                        SubnetIds=privateSubnetId,
                        env=DefaultPreference,
                        deployConfig='aws-lambda-s3.json',
                        functionName='CalcGenoTriggerFunction',
                        sourceName=bucketPlink,
                        sourceService='s3',
                        sourceArn=getBucketArn(bucketPlink),
                        FilterRules=[
                            {
                                'Name': 'suffix',
                                'Value': '.bed'
                            }
                        ])


deployLambdaEventDriven(client=clientLambda,
                        Role=arnRoles['LambdaGenoRiskAfterSimulation'],
                        dir='../../CalculateGeneticRiskLambda',
                        SecurityGroupIds=groupId,
                        SubnetIds=privateSubnetId,
                        env=DefaultPreference,
                        deployConfig='aws-lambda-dynamo.json',
                        functionName='CalcGenoRiskAfterSimFunction',
                        sourceName=tableSimulation,
                        sourceService='dynamodb',
                        sourceArn=getTableArn(Table=tableSimulation))
