# -*- coding: utf-8 -*-
from Default.defaults import *


def createRoute(client,
                DestinationCidrBlock=None,
                DestinationIpv6CidrBlock=None,
                DryRun=False,
                EgressOnlyInternetGatewayId=None,
                GatewayId=None,
                InstanceId=None,
                NatGatewayId=None,
                TransitGatewayId=None,
                NetworkInterfaceId=None,
                RouteTableId=None,
                VpcPeeringConnectionId=None):
    try:
        kwargs = locals()
        kwargs.pop('client')
        keys = kwargs.keys()
        for key in keys:
            if kwargs[key] is None:
                kwargs.pop(key)

        return client.create_route(**kwargs)

    except:
        print('Failed to create route to %s in %s route table' % (DestinationCidrBlock, RouteTableId))
        print(sys.exc_info())
