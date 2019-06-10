# -*- coding: utf-8 -*-
from Default.defaults import *


def createInternetGateway(client,
                          VpcId,
                          DryRun=False):
    try:
        return client.describe_internet_gateways(Filters=[
            {
                'Name': 'attachment.vpc-id',
                'Values': [
                    VpcId,
                ]
            }],
            DryRun=DryRun)['InternetGateways'][0]

    except:
        print('Failed to search Internet Gateway attached to %s vpc' % (VpcId))
        print(sys.exc_info())
        igw = client.create_internet_gateway(
            DryRun=DryRun
        )['InternetGateway']
        IgwId = igw['InternetGatewayId']

        response = client.attach_internet_gateway(
            DryRun=DryRun,
            InternetGatewayId=IgwId,
            VpcId=VpcId
        )
        return igw
