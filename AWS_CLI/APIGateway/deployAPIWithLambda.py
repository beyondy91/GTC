# -*- coding: utf-8 -*-

from Default.defaults import *
from Lambda.deployLambdaFunction import deployLambdaFunction
from Lambda.getLambdaFunctionArn import getLambdaFunctionArn
from .createResource import createResource
from .createRESTAPI import createRESTAPI
from .getResource import getResource
from .putMethod import putMethod
from .putIntegration import putIntegration
from .putIntegrationResponse import putIntegrationResponse
from .putMethodResponse import putMethodResponse
from .createAPIKey import createAPIKey
from .createUsagePlan import createUsagePlan
from .createUsagePlanKey import createUsagePlanKey
import tempfile


def deployAPIWithLambda(clientAPI,
                        clientLambda,
                        arnRole,
                        securityGroupIds,
                        subnetIds,
                        dir,
                        functionName,
                        apiName,
                        functionDescription,
                        parentPath,
                        targetPath,
                        stageName,
                        keyName,
                        planName,
                        deployFunction=False,
                        deployConfig='aws-lambda-tools-defaults.json',
                        env=DefaultPreference,
                        httpMethod='POST',
                        integrationHttpMethod='POST',
                        customerId='json_default',
                        endpointConfiguration={
                            'types': [
                                'REGIONAL'
                            ]
                        },
                        VpceId=None,
                        reqDescription={},
                        respDescription={}):
    result = dict()

    # lambda 생성
    if deployFunction:
        functionLambda = deployLambdaFunction(Role=arnRole,
                                              dir=dir,
                                              SecurityGroupIds=securityGroupIds,
                                              SubnetIds=subnetIds,
                                              env=env,
                                              deployConfig=deployConfig,
                                              functionName=functionName)

    if 'PRIVATE' in endpointConfiguration['types']:
        policy = DefaultAPIPolicy.replace('{{vpceID}}', VpceId)
    else:
        policy = None
    apiLambda = createRESTAPI(client=clientAPI,
                              name=apiName,
                              description=functionDescription,
                              endpointConfiguration=endpointConfiguration,
                              policy=policy)
    result['api'] = apiLambda

    apiLambdaResourceRoot = getResource(client=clientAPI,
                                        restApiId=apiLambda['id'],
                                        path=parentPath)

    if targetPath == '':
        apiLambdaResourceTarget = apiLambdaResourceRoot
    else:
        apiLambdaResourceTarget = createResource(client=clientAPI,
                                                 restApiId=
                                                 apiLambda['id'],
                                                 parentId=
                                                 apiLambdaResourceRoot['id'],
                                                 pathPart=targetPath)

    apiLambdaMethodTarget = putMethod(client=clientAPI,
                                      restApiId=apiLambda['id'],
                                      resourceId=apiLambdaResourceTarget['id'],
                                      httpMethod=httpMethod,
                                      requestParameters={
                                          "method.request.header.X-Amz-Invocation-Type": False
                                      }
                                      )

    uri_data = {
        "aws-region": locationConstraint,
        "api-version": clientLambda.meta.service_model.api_version,
        "aws-acct-id": account,
        "lambda-function-name": functionName,
    }

    uri = "arn:aws:apigateway:{aws-region}:lambda:path/{api-version}/functions/arn:aws:lambda:{aws-region}:{aws-acct-id}:function:{lambda-function-name}/invocations".format(
        **uri_data)

    # create integration
    putIntegration(client=clientAPI,
                   restApiId=apiLambda['id'],
                   resourceId=apiLambdaResourceTarget['id'],
                   httpMethod=httpMethod,
                   type="AWS",
                   integrationHttpMethod=integrationHttpMethod,
                   uri=uri,
                   requestParameters={
                       "integration.request.header.X-Amz-Invocation-Type": "method.request.header.X-Amz-Invocation-Type"
                   }
                   )

    putIntegrationResponse(client=clientAPI,
                           restApiId=apiLambda['id'],
                           resourceId=apiLambdaResourceTarget['id'],
                           integrationHttpMethod=integrationHttpMethod,
                           statusCode="200",
                           selectionPattern=".*"
                           )

    # create POST method response
    putMethodResponse(client=clientAPI,
                      restApiId=apiLambda['id'],
                      resourceId=apiLambdaResourceTarget['id'],
                      httpMethod=httpMethod,
                      statusCode="200", )

    uri_data['aws-api-id'] = apiLambda['id']
    uri_data['target-path'] = targetPath
    uri_data['http-method'] = httpMethod
    uri = "arn:aws:execute-api:{aws-region}:{aws-acct-id}:{aws-api-id}/*/{http-method}/{target-path}".format(**uri_data)

    try:
        clientLambda.add_permission(
            FunctionName=functionName,
            StatementId=uuid.uuid4().hex,
            Action="lambda:InvokeFunction",
            Principal="apigateway.amazonaws.com",
            SourceArn=uri
        )
    except:
        print('Failed to grant permission on %s to %s fail' % (functionName, uri))
        print(sys.exc_info())

    # state 'your stage name' was already created via API Gateway GUI
    try:
        clientAPI.create_deployment(
            restApiId=apiLambda['id'],
            stageName=stageName
        )
    except:
        print(sys.exc_info())

    key = createAPIKey(client=clientAPI,
                       keyName=keyName,
                       restAPIId=apiLambda['id'],
                       stageName=stageName,
                       enabled=True,
                       generateDistinctId=True,
                       customerId=customerId)
    result['key'] = key

    plan = createUsagePlan(client=clientAPI,
                           planName=planName,
                           restAPIId=apiLambda['id'],
                           stageName=stageName,
                           targetPath=apiLambdaResourceTarget['path'],
                           httpMethod=httpMethod)

    planKey = createUsagePlanKey(client=clientAPI,
                                 usagePlanId=plan['id'],
                                 keyId=key['id'])

    result['url'] = dict(
        url='https://{0}.execute-api.{1}.amazonaws.com/{2}/'.format(result['api']['id'], locationConstraint, stageName),
        key=result['key']['value'],
        description=dict(
            function=functionDescription,
            request=reqDescription,
            response=respDescription
        )
    )

    return result
