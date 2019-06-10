from Default.defaults import *

def getResource(client,
                restApiId,
                path='/'):
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
    return list(filter(lambda x: x['path'] == path, resources))[0]