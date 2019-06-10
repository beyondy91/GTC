from Default.defaults import *

def putMethod(client,
              restApiId,
              resourceId,
              httpMethod,
              authorizationType='NONE',
              apiKeyRequired=True,
              operationName='name',
              requestParameters={},
              requestModels={},
              requestValidatorId=None,
              authorizationScopes=None):
    try:
        response = client.get_method(
            restApiId=restApiId,
            resourceId=resourceId,
            httpMethod=httpMethod
        )
        return response
    except:
        print('Failed to get %s method on %s resource in %s rest api resource'%(httpMethod, resourceId, restApiId))
        print(sys.exc_info())
        kwargs = dict(
            restApiId=restApiId,
            resourceId=resourceId,
            httpMethod=httpMethod,
            authorizationType=authorizationType,
            apiKeyRequired=apiKeyRequired,
            operationName=operationName,
            requestParameters=requestParameters,
            requestModels=requestModels
        )
        if requestValidatorId != None:
            kwargs['requestValidatorId'] = requestValidatorId
        if authorizationScopes != None:
            kwargs['authorizationScopes'] = authorizationScopes
        response = client.put_method(**kwargs)
        return response