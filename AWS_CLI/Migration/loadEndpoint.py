import json, os

with open(os.path.join(os.path.dirname(os.path.abspath(__file__)), '../resource/endpointS3.json')) as inf:
    endpointS3 = json.loads(inf.read())
    endpointIdS3 = endpointS3['VpcEndpointId']


with open(os.path.join(os.path.dirname(os.path.abspath(__file__)), '../resource/endpointDynamo.json')) as inf:
    endpointDynamo = json.loads(inf.read())
    endpointIdDynamo = endpointDynamo['VpcEndpointId']

'''
with open(os.path.join(os.path.dirname(os.path.abspath(__file__)), '../resource/endpointAPIGateway.json')) as inf:
    endpointAPIGateway = json.loads(inf.read())
    endpointIdAPIGateway = endpointAPIGateway['VpcEndpointId']
'''