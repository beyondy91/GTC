from Default.defaults import *


def putIntegrationResponse(client,
                           restApiId,
                           resourceId,
                           integrationHttpMethod="POST",
                           statusCode="200",
                           selectionPattern=".*"):
    # create integration
    try:
        print(clientAPI.put_integration_response(
            restApiId=restApiId,
            resourceId=resourceId,
            httpMethod=integrationHttpMethod,
            statusCode="200",
            selectionPattern=".*"
        ))
    except:
        print(sys.exc_info())
