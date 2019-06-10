# -*- coding: utf-8 -*-
from Default.defaults import *

def createVPCEndpoint(client,
                      VpcEndpointType,
                      VpcId,
                      ServiceName,
                      RouteTableIds=None,
                      SubnetIds=None,
                      SecurityGroupIds=None
                      ):
    try:
        response = client.describe_vpc_endpoints(
            DryRun=False,
            Filters=[
                {
                    'Name': 'vpc-id',
                    'Values': [
                        VpcId,
                    ]
                },
                {
                    'Name': 'service-name',
                    'Values': [
                        ServiceName,
                    ]
                },
            ],
        )

        return response['VpcEndpoints'][0]
    except:
        print('Failed to get %s endpoint in %s vpc'%(ServiceName, VpcId))
        print(sys.exc_info())
        kwargs = dict(DryRun=False,
                      VpcEndpointType=VpcEndpointType,
                      VpcId=VpcId,
                      ServiceName=ServiceName)
        if RouteTableIds != None:
            kwargs['RouteTableIds'] = RouteTableIds
        if SubnetIds != None:
            kwargs['SubnetIds'] = SubnetIds
        if SecurityGroupIds != None:
            kwargs['SecurityGroupIds'] = SecurityGroupIds
        response = client.create_vpc_endpoint(**kwargs)
        return response['VpcEndpoint']