# -*- coding: utf-8 -*-
from Default.defaults import *

def createSubnet(client,
                 CidrBlock,
                 VpcId='string',
                 DryRun=False,
                 name='json_default'):
    try:
        response = client.create_subnet(
            CidrBlock=CidrBlock,
            VpcId=VpcId,
            DryRun=DryRun
        )
        SubnetId = response['Subnet']['SubnetId']
        SubnetArn = response['Subnet']['SubnetArn']
        client.create_tags(DryRun=DryRun,
                           Resources=[SubnetId],
                           Tags=[{'Key': 'Name', 'Value': name},
                                 {'Key': 'VpcId', 'Value': VpcId}])
        return response['Subnet']

    except:
        print('Failed to create %s subnet' % (CidrBlock))
        print(sys.exc_info())
        response = client.describe_subnets(
            Filters=[
                {
                    'Name': 'tag:Name',
                    'Values': [
                        name,
                    ]
                },
                {
                    'Name': 'tag:VpcId',
                    'Values': [
                        VpcId
                    ]
                }
            ],
            DryRun=DryRun
        )
        SubnetId = response['Subnets'][0]['SubnetId']
        SubnetArn = response['Subnets'][0]['SubnetArn']

    return response['Subnets'][0]