# -*- coding: utf-8 -*-
from __future__ import print_function
import sys, os

sys.path.append(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))
os.chdir(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))

from Default.defaults import *
from json_default import json_default
from IAM.createPolicy import createPolicy

arnPolicies = dict()

# GTC 버켓 읽기 권한 정책 생성
arnPolicies['LambdaReadS3GTCPolicy'] = createPolicy(client=clientIAM,
                                                    PolicyName='LambdaReadS3GTCPolicy',
                                                    Path=pathDefault,
                                                    policyFilename='JSON/LambdaReadS3GTCPolicy.json',
                                                    Description='Policy to read GTC files from S3 bucket')['Arn']

# GTC 버켓 읽기/쓰기 권한 정책 생성
arnPolicies['LambdaRWS3GTCPolicy'] = createPolicy(client=clientIAM,
                                                  PolicyName='LambdaRWS3GTCPolicy',
                                                  Path=pathDefault,
                                                  policyFilename='JSON/LambdaRWS3GTCPolicy.json',
                                                  Description='Policy to RW GTC files from S3 bucket')['Arn']

# BPM 버켓 읽기 권한 정책 생성
arnPolicies['LambdaReadS3BPMPolicy'] = createPolicy(client=clientIAM,
                                                    PolicyName='LambdaReadS3BPMPolicy',
                                                    Path=pathDefault,
                                                    policyFilename='JSON/LambdaReadS3BPMPolicy.json',
                                                    Description='Policy to read BPM files from S3 bucket')['Arn']

# BPM 버켓 읽기/쓰기 권한 정책 생성
arnPolicies['LambdaRWS3BPMPolicy'] = createPolicy(client=clientIAM,
                                                  PolicyName='LambdaRWS3BPMPolicy',
                                                  Path=pathDefault,
                                                  policyFilename='JSON/LambdaRWS3BPMPolicy.json',
                                                  Description='Policy to RW BPM files from S3 bucket')['Arn']

# Plink 버켓 읽기/쓰기 권한 정책 생성
arnPolicies['LambdaReadS3PlinkPolicy'] = createPolicy(client=clientIAM,
                                                      PolicyName='LambdaReadS3PlinkPolicy',
                                                      Path=pathDefault,
                                                      policyFilename='JSON/LambdaReadS3PlinkPolicy.json',
                                                      Description='Policy to r/w Plink files from S3 bucket')['Arn']

# Plink 버켓 읽기/쓰기 권한 정책 생성
arnPolicies['LambdaRWS3PlinkPolicy'] = createPolicy(client=clientIAM,
                                                    PolicyName='LambdaRWS3PlinkPolicy',
                                                    Path=pathDefault,
                                                    policyFilename='JSON/LambdaRWS3PlinkPolicy.json',
                                                    Description='Policy to r/w Plink files from S3 bucket')['Arn']

# chain 버켓 읽기 권한 정책 생성
arnPolicies['LambdaReadS3chainPolicy'] = createPolicy(client=clientIAM,
                                                      PolicyName='LambdaReadS3ChainPolicy',
                                                      Path=pathDefault,
                                                      policyFilename='JSON/LambdaReadS3ChainPolicy.json',
                                                      Description='Policy to read chain files from S3 bucket')['Arn']

# Simulation 버켓 읽기/쓰기 권한 정책 생성
arnPolicies['LambdaReadS3SimulationPolicy'] = createPolicy(client=clientIAM,
                                                           PolicyName='LambdaReadS3SimulationPolicy',
                                                           Path=pathDefault,
                                                           policyFilename='JSON/LambdaReadS3SimulationPolicy.json',
                                                           Description='Policy to read Simulation results from S3 bucket')[
    'Arn']

# Simulation 버켓 읽기/쓰기 권한 정책 생성
arnPolicies['LambdaRWS3SimulationPolicy'] = createPolicy(client=clientIAM,
                                                         PolicyName='LambdaRWS3SimulationPolicy',
                                                         Path=pathDefault,
                                                         policyFilename='JSON/LambdaRWS3SimulationPolicy.json',
                                                         Description='Policy to r/w Simulation results from S3 bucket')[
    'Arn']

# network interface 생성 권한 정책 생성
arnPolicies['LambdaCreateNetworkInterface'] = createPolicy(client=clientIAM,
                                                           PolicyName='LambdaEC2CreateNetworkInterfacePolicy',
                                                           policyFilename='JSON/LambdaCreateNetworkInterfacePolicy.json',
                                                           Description='Policy to r/w Progress Table (DynamoDB)',
                                                           Path=pathDefault)['Arn']

# g1preference 읽기 권한 정책 생성
arnPolicies['LambdaReadg1preferencePolicy'] = createPolicy(client=clientIAM,
                                                           PolicyName='LambdaReadg1preferencePolicy',
                                                           Path=pathDefault,
                                                           policyFilename='JSON/LambdaReadg1preferencePolicy.json',
                                                           Description='Policy to read table with preferences')['Arn']

# g1manifest 읽기 권한 정책 생성
arnPolicies['LambdaReadg1manifestPolicy'] = createPolicy(client=clientIAM,
                                                         PolicyName='LambdaReadg1manifestPolicy',
                                                         Path=pathDefault,
                                                         policyFilename='JSON/LambdaReadg1manifestPolicy.json',
                                                         Description='Policy to read table with manifests')['Arn']

# g1progress 읽기/쓰기 권한 정책 생성
arnPolicies['LambdaRWg1progressPolicy'] = createPolicy(client=clientIAM,
                                                       PolicyName='LambdaRWg1progressPolicy',
                                                       Path=pathDefault,
                                                       policyFilename='JSON/LambdaRWg1progressPolicy.json',
                                                       Description='Policy to r/w Plink files from S3 bucket')['Arn']

# g1filename 읽기/쓰기 권한 정책 생성
arnPolicies['LambdaRWg1filenamePolicy'] = createPolicy(client=clientIAM,
                                                       PolicyName='LambdaRWg1filenamePolicy',
                                                       Path=pathDefault,
                                                       policyFilename='JSON/LambdaRWg1filenamePolicy.json',
                                                       Description='Policy to r/w table with filename-serial number matching info')[
    'Arn']

# g1metagenotype 읽기 권한 정책 생성
arnPolicies['LambdaReadg1metagenotypePolicy'] = createPolicy(client=clientIAM,
                                                             PolicyName='LambdaReadg1metagenotypePolicy',
                                                             Path=pathDefault,
                                                             policyFilename='JSON/LambdaReadg1metagenotypePolicy.json',
                                                             Description='Policy to read table with Genotype meta')[
    'Arn']

# g1metagenotype 읽기/쓰기 권한 정책 생성
arnPolicies['LambdaRWg1metagenotypePolicy'] = createPolicy(client=clientIAM,
                                                           PolicyName='LambdaRWg1metagenotypePolicy',
                                                           Path=pathDefault,
                                                           policyFilename='JSON/LambdaRWg1metagenotypePolicy.json',
                                                           Description='Policy to r/w table with Genotype meta')['Arn']

# g1metasurvey 읽기 권한 정책 생성
arnPolicies['LambdaReadg1metasurveyPolicy'] = createPolicy(client=clientIAM,
                                                           PolicyName='LambdaReadg1metasurveyPolicy',
                                                           Path=pathDefault,
                                                           policyFilename='JSON/LambdaReadg1metasurveyPolicy.json',
                                                           Description='Policy to read table with survey meta')['Arn']

# g1metasurvey 읽기/쓰기 권한 정책 생성
arnPolicies['LambdaRWg1metasurveyPolicy'] = createPolicy(client=clientIAM,
                                                         PolicyName='LambdaRWg1metasurveyPolicy',
                                                         Path=pathDefault,
                                                         policyFilename='JSON/LambdaRWg1metasurveyPolicy.json',
                                                         Description='Policy to r/w table with survey meta')['Arn']

# g1meta 읽기 권한 정책 생성
arnPolicies['LambdaReadg1metaPolicy'] = createPolicy(client=clientIAM,
                                                     PolicyName='LambdaReadg1metaPolicy',
                                                     Path=pathDefault,
                                                     policyFilename='JSON/LambdaReadg1metaPolicy.json',
                                                     Description='Policy to read table with survey meta')['Arn']

# g1meta 읽기/쓰기 권한 정책 생성
arnPolicies['LambdaRWg1metaPolicy'] = createPolicy(client=clientIAM,
                                                   PolicyName='LambdaRWg1metaPolicy',
                                                   Path=pathDefault,
                                                   policyFilename='JSON/LambdaRWg1metaPolicy.json',
                                                   Description='Policy to r/w table with survey meta')['Arn']

# g1result 읽기 권한 정책 생성
arnPolicies['LambdaReadg1resultPolicy'] = createPolicy(client=clientIAM,
                                                       PolicyName='LambdaReadg1resultPolicy',
                                                       Path=pathDefault,
                                                       policyFilename='JSON/LambdaReadg1resultPolicy.json',
                                                       Description='Policy to read table with result')['Arn']

# g1result 읽기/쓰기 권한 정책 생성
arnPolicies['LambdaRWg1resultPolicy'] = createPolicy(client=clientIAM,
                                                     PolicyName='LambdaRWg1resultPolicy',
                                                     Path=pathDefault,
                                                     policyFilename='JSON/LambdaRWg1resultPolicy.json',
                                                     Description='Policy to r/w table with result')['Arn']

# Logging 권한 정책 생성
arnPolicies['LambdaLog'] = createPolicy(client=clientIAM,
                                        PolicyName='LambdaLogPolicy',
                                        Path=pathDefault,
                                        policyFilename='JSON/LambdaLogPolicy.json',
                                        Description='Policy to write log in cloudwatch')['Arn']

# g1simulation 읽기 권한 정책 생성
arnPolicies['LambdaReadg1simulationPolicy'] = createPolicy(client=clientIAM,
                                                           PolicyName='LambdaReadg1simulationPolicy',
                                                           Path=pathDefault,
                                                           policyFilename='JSON/LambdaReadg1simulationPolicy.json',
                                                           Description='Policy to read g1simulation table')['Arn']

# g1simulation 읽기/쓰기 권한 정책 생성
arnPolicies['LambdaRWg1simulationPolicy'] = createPolicy(client=clientIAM,
                                                         PolicyName='LambdaRWg1simulationPolicy',
                                                         Path=pathDefault,
                                                         policyFilename='JSON/LambdaRWg1simulationPolicy.json',
                                                         Description='Policy to r/w g1simulation table')['Arn']

with open('resource/arnPolicy.json', 'w') as outf:
    outf.write(json.dumps(arnPolicies, default=json_default))
