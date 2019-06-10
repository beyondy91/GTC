from Default.defaults import *


def createAPIKey(client,
                 keyName,
                 restAPIId,
                 stageName,
                 enabled=True,
                 generateDistinctId=True,
                 customerId='string'):
    try:
        key = list(filter(lambda x: x['stageKeys'] == '%s/%s' % (restAPIId, stageName), client.get_api_keys(
            nameQuery=keyName,
            customerId=customerId,
            limit=500
        )['items']))[0]
    except:
        key = clientAPI.create_api_key(
            name=keyName,
            enabled=True,
            generateDistinctId=True,
            stageKeys=[
                {
                    'restApiId': restAPIId,
                    'stageName': stageName
                },
            ],
            customerId=customerId
        )
    return key