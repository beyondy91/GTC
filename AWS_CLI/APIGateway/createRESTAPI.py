from Default.defaults import *

def createRESTAPI(client,
                  name,
                  description,
                  binaryMediaTypes=[],
                  apiKeySource='HEADER',
                  endpointConfiguration={
                      'types': [
                          'PRIVATE'
                      ]
                  },
                  policy=None):
    try:
        rest_apis = list()
        kwargs = dict(limit=500)
        while True:
            response = client.get_rest_apis(**kwargs)
            if 'position' in response:
                kwargs['position'] = response['position']
            rest_apis.extend(response['items'])
            if len(response['items']) < 500:
                break
        return list(filter(lambda x: x['name'] == name, rest_apis))[0]
    except:
        print('Failed to get %s rest api'%(name))
        print(sys.exc_info())
        kwargs = dict(
            name=name,
            description=description,
            binaryMediaTypes=binaryMediaTypes,
            apiKeySource=apiKeySource,
            endpointConfiguration=endpointConfiguration
        )
        if policy is not None:
            kwargs['policy'] = policy
        response = client.create_rest_api(
            **kwargs
        )
        return response