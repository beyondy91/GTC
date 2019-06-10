# -*- coding: utf-8 -*-
from Default.defaults import *
from time import sleep

def createNATGateWay(client,
                     VpcId,
                     SubnetId,
                     DryRun=False):
    try:
        return client.describe_nat_gateways(
            Filters=[
                {
                    'Name': 'subnet-id',
                    'Values': [
                        SubnetId
                    ]
                },
                {
                    'Name': 'vpc-id',
                    'Values': [
                        VpcId
                    ]
                },
            ]
        )['NatGateways'][0]

    except:
        print('Failed to search NAT Gateway attached to %s vpc and %s subnet' % (VpcId, SubnetId))
        print(sys.exc_info())
        eip = client.allocate_address(
            Domain='vpc',
            DryRun=DryRun
        )

        response = client.create_nat_gateway(
            AllocationId=eip['AllocationId'],
            SubnetId=SubnetId
        )

        sleep(300)

        return response['NatGateway']
