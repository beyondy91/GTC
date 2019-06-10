# -*- coding: utf-8 -*-
from __future__ import print_function
import sys, os

sys.path.append(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))
os.chdir(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))

from Default.defaults import *
from json_default import json_default
from IAM.createRole import createRole

arnRoles = dict()

# GTC를 Plink로 변환하는 Lambda 는 GTC, BPM 버켓의 읽기, Plink 버켓의 읽기, 쓰기 권한이 필요함
arnRoles['LambdaS3GTCToPlink'] = createRole(client=clientIAM,
                                            RoleName='LambdaS3GTCToPlink',
                                            assume_role_file='JSON/LambdaS3GTCToPlinkAssumeRole.json',
                                            Description='Role for Lambda function to convert GTC to Plink from S3',
                                            Path=pathDefault)['Arn']

# BPM을 BIM으로 변환하는 Lambda 는 GTC, BPM 버켓의 읽기, Plink 버켓의 읽기, 쓰기 권한이 필요함
arnRoles['LambdaS3BPMToBIM'] = createRole(client=clientIAM,
                                          RoleName='LambdaS3BPMToBIM',
                                          assume_role_file='JSON/LambdaS3BPMToBIMAssumeRole.json',
                                          Description='Role for Lambda function to convert BPM to BIM from S3',
                                          Path=pathDefault)['Arn']

# Monte-Carlo simulation Lambda용 Role
arnRoles['LambdaSimulation'] = createRole(client=clientIAM,
                                          RoleName='LambdaSimulation',
                                          assume_role_file='JSON/LambdaSimulationAssumeRole.json',
                                          Description='Role for Lambda function to run Monte-Carlo Simulation',
                                          Path=pathDefault)['Arn']

# 설문 simulation cache용
arnRoles['LambdaSurveySimulation'] = createRole(client=clientIAM,
                                                RoleName='LambdaSurveySimulation',
                                                assume_role_file='JSON/LambdaSurveySimulationAssumeRole.json',
                                                Description='Role for Lambda function to cache Monte-Carlo Simulation results for survey',
                                                Path=pathDefault)['Arn']

# LiftOver Lambda용 Role
arnRoles['LambdaLiftOver'] = createRole(client=clientIAM,
                                        RoleName='LambdaLiftOver',
                                        assume_role_file='JSON/LambdaLiftOverAssumeRole.json',
                                        Description='Role for Lambda function to run LiftOver',
                                        Path=pathDefault)['Arn']

# GenotypeSimulation Lambda용 Role
arnRoles['LambdaGenotypeSimulation'] = createRole(client=clientIAM,
                                                  RoleName='LambdaGenotypeSimulation',
                                                  assume_role_file='JSON/LambdaLiftOverAssumeRole.json',
                                                  Description='Role for Lambda function to run genotype simulation',
                                                  Path=pathDefault)['Arn']

# ListTrait Lambda용 Role
arnRoles['LambdaListTrait'] = createRole(client=clientIAM,
                                         RoleName='LambdaListTrait',
                                         assume_role_file='JSON/LambdaListTraitAssumeRole.json',
                                         Description='Role for Lambda function to retrieve trait meta list',
                                         Path=pathDefault)['Arn']

# CalcGenoRisk Lambda용 Role
arnRoles['LambdaCalcGenoRisk'] = createRole(client=clientIAM,
                                            RoleName='LambdaCalcGenoRisk',
                                            assume_role_file='JSON/LambdaDefaultAssumeRole.json',
                                            Description='Role for Lambda function to calculate the genetic risk',
                                            Path=pathDefault)['Arn']

# GenoRiskTrigger Lambda용 Role
arnRoles['LambdaGenoRiskTrigger'] = createRole(client=clientIAM,
                                               RoleName='LambdaGenoRiskTrigger',
                                               assume_role_file='JSON/LambdaDefaultAssumeRole.json',
                                               Description='Role for Lambda function to trigger the genetic risk calculation',
                                               Path=pathDefault)['Arn']

# GenoRiskAfterSimulation Lambda용 Role
arnRoles['LambdaGenoRiskAfterSimulation'] = createRole(client=clientIAM,
                                                       RoleName='LambdaGenoRiskAfterSimulation',
                                                       assume_role_file='JSON/LambdaDefaultAssumeRole.json',
                                                       Description='Role for Lambda function to calculate the genetic risk after simulation',
                                                       Path=pathDefault)['Arn']

# GTCSNChange Lambda용 Role
arnRoles['LambdaGTCSNChange'] = createRole(client=clientIAM,
                                           RoleName='LambdaGTCSNChange',
                                           assume_role_file='JSON/LambdaDefaultAssumeRole.json',
                                           Description='Role for Lambda function to change SN in GTC file',
                                           Path=pathDefault)['Arn']

with open('resource/arnRole.json', 'w') as outf:
    outf.write(json.dumps(arnRoles, default=json_default))
