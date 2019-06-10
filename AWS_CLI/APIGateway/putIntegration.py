from Default.defaults import *


def putIntegration(client,
                   restApiId,
                   resourceId,
                   httpMethod="POST",
                   type="AWS",
                   integrationHttpMethod="POST",
                   uri='',
                   requestParameters={
                       "integration.request.header.X-Amz-Invocation-Type": "method.request.header.X-Amz-Invocation-Type"
                   }):
    # create integration
    try:
        print(client.put_integration(
            restApiId=restApiId,
            resourceId=resourceId,
            httpMethod=httpMethod,
            type=type,
            integrationHttpMethod=integrationHttpMethod,
            uri=uri,
            requestParameters=requestParameters
        ))
    except:
        print(sys.exc_info())
