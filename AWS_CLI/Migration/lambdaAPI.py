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
from loadVPC import *
from loadEndpoint import *
from APIGateway.deployAPIWithLambda import deployAPIWithLambda
from json_default import json_default

# api url, key 저장하기 위한 preference set
apiPreferences = dict()

# api gateway와 lambda를 통합
apiLambdaGtcToPlinkS3 = deployAPIWithLambda(clientAPI=clientAPI,
                                            clientLambda=clientLambda,
                                            arnRole=arnRoles['LambdaS3GTCToPlink'],
                                            securityGroupIds=groupId,
                                            subnetIds=privateSubnetId,
                                            dir='../../GTCToPlinkS3APILambda',
                                            functionName='GTCToPlinkAPIFunction',
                                            apiName='GTCToPlinkAPIFunction-API',
                                            functionDescription='API to convert GTC to Plink file format in S3',
                                            parentPath='/',
                                            targetPath='',
                                            stageName='convert',
                                            keyName='convertKey',
                                            planName='convertPlan',
                                            deployFunction=True,
                                            reqDescription=dict(header={'X-Api-Key': 'API key',
                                                                        'X-Amz-Invocation-Type': 'Event (for async)'},
                                                                body={"GTCKeys": [
                                                                    "(string) gtc filenames for conversion"]})
                                            )
apiPreferences['InternalGTCToPlinkAPIFunction-API'] = apiLambdaGtcToPlinkS3['url']

# api gateway와 lambda를 통합
apiLambdaBPMToBIM = deployAPIWithLambda(clientAPI=clientAPI,
                                        clientLambda=clientLambda,
                                        arnRole=arnRoles['LambdaS3BPMToBIM'],
                                        securityGroupIds=groupId,
                                        subnetIds=privateSubnetId,
                                        dir='../../BPMToBIMAPILambda',
                                        functionName='BPMToBIMAPIFunction',
                                        apiName='BPMToBIMAPIFunction-API',
                                        functionDescription='API to convert BPM manifest to BIM in S3',
                                        parentPath='/',
                                        targetPath='',
                                        stageName='convert',
                                        keyName='bpmToBimConvertKey',
                                        planName='bpmToBimConvertPlan',
                                        deployFunction=True,
                                        reqDescription=dict(header={'X-Api-Key': 'API key',
                                                                    'X-Amz-Invocation-Type': 'Event (for async)'},
                                                            body={"BPMKey": "(string) bpm filename for conversion"}))
apiPreferences['InternalBPMToBIMAPIFunction-API'] = apiLambdaBPMToBIM['url']

# api gateway와 lambda를 통합
apiLambdaSimulation = deployAPIWithLambda(clientAPI=clientAPI,
                                          clientLambda=clientLambda,
                                          arnRole=arnRoles['LambdaSimulation'],
                                          securityGroupIds=groupId,
                                          subnetIds=privateSubnetId,
                                          dir='../../SimulationAPILambda',
                                          functionName='SimulationAPIFunction',
                                          apiName='SimulationAPIFunction-API',
                                          functionDescription='API to run Monte-Carlo simulation for calculate the risk scores compared to the average population',
                                          parentPath='/',
                                          targetPath='',
                                          stageName='simulation',
                                          keyName='simulationRunKey',
                                          planName='simulationRunPlan',
                                          deployFunction=True,
                                          reqDescription=dict(header={'X-Api-Key': 'API key',
                                                                      'X-Amz-Invocation-Type': 'Event (for async)'},
                                                              body={"sn": "(string) serial number",
                                                                    "code": "(string) trait code",
                                                                    "type": "(string) genotype|survey",
                                                                    "size (optional)": "(int) Size of Monte-Carlo simulation",
                                                                    "meta": [{"sex": "(string) M|F",
                                                                              "var": "(string) variable name (sid for survey, snpname for genotype)",
                                                                              "freq": "(float) frequency of risk allele/factor",
                                                                              "beta": "(float) beta coefficients of risk allele/factor"}]}))
apiPreferences['InternalSimulationAPIFunction-API'] = apiLambdaSimulation['url']

apiLambdaSurveySimulation = deployAPIWithLambda(clientAPI=clientAPI,
                                                clientLambda=clientLambda,
                                                arnRole=arnRoles['LambdaSurveySimulation'],
                                                securityGroupIds=groupId,
                                                subnetIds=privateSubnetId,
                                                dir='../../SurveySimulationAPILambda',
                                                functionName='SurveySimAPIFunction',
                                                apiName='SurveySimAPIFunction-API',
                                                functionDescription='API to cache the result of Monte-Carlo simulation for every possible combination of survey answers',
                                                parentPath='/',
                                                targetPath='',
                                                stageName='simulation',
                                                keyName='surveySimulationRunKey',
                                                planName='surveySimulationRunPlan',
                                                deployFunction=True,
                                                reqDescription=dict(header={'X-Api-Key': 'API key',
                                                                            'X-Amz-Invocation-Type': 'Event (for async)'},
                                                                    body={"code": "(string) trait code",
                                                                          "size (optional)": "(int) Size of Monte-Carlo simulation",
                                                                          "sex": "(string) M|F",
                                                                          "meta": [{
                                                                              "var": "(string) variable name (sid for survey, snpname for genotype)",
                                                                              "freq": "(float) frequency of risk allele/factor",
                                                                              "beta": "(float) beta coefficients of risk allele/factor"}]})
                                                )
apiPreferences['InternalSurveySimAPIFunction-API'] = apiLambdaSurveySimulation['url']

apiLambdaSurveySimulationAPI = deployAPIWithLambda(clientAPI=clientAPI,
                                                   clientLambda=clientLambda,
                                                   arnRole=arnRoles['LambdaSurveySimulation'],
                                                   securityGroupIds=groupId,
                                                   subnetIds=privateSubnetId,
                                                   dir='../../SurveySimulationDynamoLambda',
                                                   functionName='SurveySimDynAPIFunction',
                                                   apiName='SurveySimDynAPIFunction-API',
                                                   functionDescription='API to cache the result of Monte-Carlo simulation for every possible combination of survey answers based on the DynamoDB entries',
                                                   parentPath='/',
                                                   targetPath='',
                                                   stageName='simulation',
                                                   keyName='surveySimulationDynamoRunKey',
                                                   planName='surveySimulationDynamoRunPlan',
                                                   deployConfig='aws-lambda-api.json',
                                                   deployFunction=True,
                                                   reqDescription=dict(header={'X-Api-Key': 'API key',
                                                                               'X-Amz-Invocation-Type': 'Event (for async)'},
                                                                       body={"code": "(string) trait code"}))
apiPreferences['InternalSurveySimDynAPIFunction-API'] = apiLambdaSurveySimulationAPI['url']

# LiftOver lambda/api gateway 배포
apiLambdaLiftover = deployAPIWithLambda(clientAPI=clientAPI,
                                        clientLambda=clientLambda,
                                        arnRole=arnRoles['LambdaLiftOver'],
                                        securityGroupIds=groupId,
                                        subnetIds=privateSubnetId,
                                        dir='../../LiftOverAPILambda',
                                        functionName='LiftOverAPIFunction',
                                        apiName='LiftOverAPIFunction-API',
                                        functionDescription='API to lift over BIM file in S3',
                                        parentPath='/',
                                        targetPath='',
                                        stageName='liftover',
                                        keyName='liftoverKey',
                                        planName='liftOverRunPlan',
                                        deployFunction=True,
                                        reqDescription=dict(header={'X-Api-Key': 'API key',
                                                                    'X-Amz-Invocation-Type': 'Event (for async)'},
                                                            body={"chainKey": "(string) chain filename",
                                                                  "bimKey": "(string) original BIM filename",
                                                                  "bimKeyNew": "(string) lifted BIM filename",
                                                                  "chainBucketRegion (optional)": "(string) chain file bucket region",
                                                                  "chainBucketName (optional)": "(string) chain file bucket name",
                                                                  "bimBucketRegion (optional)": "(string) BIM file bucket region",
                                                                  "bimBucketName (optional)": "(string) BIM file bucket name"}))
apiPreferences['InternalLiftOverAPIFunction-API'] = apiLambdaLiftover['url']

# GenotypeSimulation lambda/api gateway 배포
apiLambdaGenotypeSimulation = deployAPIWithLambda(clientAPI=clientAPI,
                                                  clientLambda=clientLambda,
                                                  arnRole=arnRoles['LambdaGenotypeSimulation'],
                                                  securityGroupIds=groupId,
                                                  subnetIds=privateSubnetId,
                                                  dir='../../GenotypeSimulationLambda',
                                                  functionName='GenoSimulationAPIFunction',
                                                  apiName='GenoSimulationAPIFunction-API',
                                                  functionDescription='API to run the Monte-Carlo simulation for genotype data in S3',
                                                  parentPath='/',
                                                  targetPath='',
                                                  stageName='simulation',
                                                  keyName='genoSimulationKey',
                                                  planName='genoSimulationPlan',
                                                  deployFunction=True,
                                                  deployConfig='aws-lambda-api.json',
                                                  reqDescription=dict(header={'X-Api-Key': 'API key',
                                                                              'X-Amz-Invocation-Type': 'Event (for async)'},
                                                                      body={"sn": "(string) serial number"}))
apiPreferences['InternalGenoSimulationAPIFunction-API'] = apiLambdaGenotypeSimulation['url']

# ListTrait Get lambda/api gateway 배포
apiLambdaListTrait = deployAPIWithLambda(clientAPI=clientAPI,
                                         clientLambda=clientLambda,
                                         arnRole=arnRoles['LambdaListTrait'],
                                         securityGroupIds=groupId,
                                         subnetIds=privateSubnetId,
                                         dir='../../ListTraitAPILambda',
                                         functionName='ListTraitAPIFunction',
                                         apiName='ListTraitAPIFunction-API',
                                         functionDescription='API to retrieve the list of all traits',
                                         parentPath='/',
                                         targetPath='',
                                         stageName='list',
                                         keyName='ListTraitGetKey',
                                         planName='ListTraitGetPlan',
                                         deployFunction=True,
                                         deployConfig='aws-lambda-api-get.json',
                                         httpMethod='GET',
                                         integrationHttpMethod='POST',
                                         reqDescription=dict(header={'X-Api-Key': 'API key'}),
                                         respDescription={"code": "(string) trait code",
                                                          "meta": {"Kor": {
                                                              "name": "(string) trait name in Korean",
                                                              "description": "(string) trait description in Korean"},
                                                              "Eng": {
                                                                  "name": "(string) trait name in English",
                                                                  "description": "(string) trait description in English"},
                                                              "Jpn": {
                                                                  "name": "(string) trait name in Japanese",
                                                                  "description": "(string) trait description in Japanese"}}}
                                         )
apiPreferences['OpenListTraitAPIFunction-API'] = apiLambdaListTrait['url']

# CalcGenoRisk Post lambda/api gateway 배포
apiLambdaCalcGenoRisk = deployAPIWithLambda(clientAPI=clientAPI,
                                            clientLambda=clientLambda,
                                            arnRole=arnRoles['LambdaCalcGenoRisk'],
                                            securityGroupIds=groupId,
                                            subnetIds=privateSubnetId,
                                            dir='../../CalculateGeneticRiskLambda',
                                            functionName='CalcGenoRiskAPIFunction',
                                            apiName='CalcGenoRiskAPIFunction-API',
                                            functionDescription='API to calculate the individual risk from genotype data in S3 to DynamoDB',
                                            parentPath='/',
                                            targetPath='',
                                            stageName='calc',
                                            keyName='CalcGenoRiskPostKey',
                                            planName='CalcGenoRiskPostPlan',
                                            deployFunction=True,
                                            deployConfig='aws-lambda-api-post.json',
                                            httpMethod='POST',
                                            integrationHttpMethod='POST',
                                            reqDescription=dict(header={'X-Api-Key': 'API key',
                                                                        'X-Amz-Invocation-Type': 'Event (for async)'},
                                                                body={"sn": "(string) serial number",
                                                                      "codes (optional)": ["(string) trait code"]}))
apiPreferences['InternalCalcGenoRiskAPIFunction-API'] = apiLambdaCalcGenoRisk['url']

# GTCSNChange Get lambda/api gateway 배포
apiLambdaGTCSNChange = deployAPIWithLambda(clientAPI=clientAPI,
                                           clientLambda=clientLambda,
                                           arnRole=arnRoles['LambdaGTCSNChange'],
                                           securityGroupIds=groupId,
                                           subnetIds=privateSubnetId,
                                           dir='../../GTCSampleNameChangeAPILambda',
                                           functionName='GTCSNChangeAPIFunction',
                                           apiName='GTCSNChangeAPIFunction-API',
                                           functionDescription='API to change the sample name in GTC file',
                                           parentPath='/',
                                           targetPath='',
                                           stageName='change',
                                           keyName='GTCSNChangePostKey',
                                           planName='GTCSNChangePostPlan',
                                           deployFunction=True,
                                           deployConfig='aws-lambda-api-post.json',
                                           httpMethod='POST',
                                           integrationHttpMethod='POST',
                                           reqDescription=dict(header={'X-Api-Key': 'API key',
                                                                       'X-Amz-Invocation-Type': 'Event (for async)'},
                                                               body={"sn": "(string) serial number",
                                                                     "codes (optional)": ["(string) trait code"]}))
apiPreferences['InternalGTCSNChangeAPIFunction-API'] = apiLambdaGTCSNChange['url']

table = resourceDynamoDB.Table(tablePreference)
table.put_item(
    Item={
        'key': 'API',
        'preference': json.dumps(apiPreferences)
    }
)

with open('resource/api.json', 'w') as outf:
    outf.write(json.dumps(apiPreferences, default=json_default))
