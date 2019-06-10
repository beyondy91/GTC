from Default.defaults import *


def putMethodResponse(client,
                      restApiId,
                      resourceId,
                      httpMethod="POST",
                      statusCode="200", ):
    # create integration
    try:
        print(clientAPI.put_method_response(
            restApiId=restApiId,
            resourceId=resourceId,
            httpMethod=httpMethod,
            statusCode=statusCode,
        ))
    except:
        print(sys.exc_info())
