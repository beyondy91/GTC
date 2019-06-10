# -*- coding: utf-8 -*-
from __future__ import print_function
import sys, os

sys.path.append(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))
os.chdir(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))

from Default.defaults import *

from Network.createVPCEndpoint import createVPCEndpoint
from loadVPC import *
from loadPrivateSubnet import *
from loadPublicSubnet import *
from json_default import json_default

# S3용 endpoint 생성
endpointS3 = createVPCEndpoint(client=clientEC2,
                               VpcEndpointType='Gateway',
                               VpcId=vpcId,
                               ServiceName='com.amazonaws.%s.s3' % (locationConstraint),
                               RouteTableIds=[privateRouteTableId])
endpointIdS3 = endpointS3['VpcEndpointId']

# DynamoDB용 endpoint 생성
endpointDynamo = createVPCEndpoint(client=clientEC2,
                                   VpcEndpointType='Gateway',
                                   VpcId=vpcId,
                                   ServiceName='com.amazonaws.%s.dynamodb' % (locationConstraint),
                                   RouteTableIds=[privateRouteTableId])
endpointIdDynamo = endpointDynamo['VpcEndpointId']

# APIGateway endpoint 생성
'''
endpointAPIGateway = createVPCEndpoint(client=clientEC2,
                                       VpcEndpointType='Interface',
                                       VpcId=vpcId,
                                       ServiceName='com.amazonaws.%s.execute-api' % (locationConstraint),
                                       SubnetIds=[publicSubnetId])
endpointIdAPIGateway = endpointAPIGateway['VpcEndpointId']
'''

with open('resource/endpointS3.json', 'w') as outf:
    outf.write(json.dumps(endpointS3, default=json_default))

with open('resource/endpointDynamo.json', 'w') as outf:
    outf.write(json.dumps(endpointDynamo, default=json_default))
'''
with open('resource/endpointAPIGateway.json', 'w') as outf:
    outf.write(json.dumps(endpointAPIGateway, default=json_default))
'''