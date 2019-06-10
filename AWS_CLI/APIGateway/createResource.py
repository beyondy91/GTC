from Default.defaults import *

def createResource(client,
                restApiId,
                parentId,
                pathPart):
    try:
        resources = list()
        kwargs = dict(limit=500,
                      restApiId=restApiId)
        while True:
            response = client.get_resources(**kwargs)
            if 'position' in response:
                kwargs['position'] = response['position']
            resources.extend(response['items'])
            if len(response['items']) < 500:
                break
        return list(filter(lambda x: 'pathPart' in x and x['pathPart'] == pathPart, resources))[0]
    except:
        print('Failed to get %s resource in %s rest api resource'%(pathPart, restApiId))
        print(sys.exc_info())
        response = client.create_resource(
            restApiId=restApiId,
            parentId=parentId,
            pathPart=pathPart
        )
        return response