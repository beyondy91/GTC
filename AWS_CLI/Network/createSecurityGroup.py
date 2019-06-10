# -*- coding: utf-8 -*-
from Default.defaults import *

def createSecurityGroup(client,
                        Description='string',
                        GroupName='string',
                        VpcId='string',
                        DryRun=False):
    try:
        response = client.create_security_group(
            Description=Description,
            GroupName=GroupName,
            VpcId=VpcId,
            DryRun=DryRun
        )
        GroupId = response

    except:
        print('Failed to create %s security group' % (GroupName))
        print(sys.exc_info())
        response = client.describe_security_groups(
            Filters=[
                {
                    'Name': 'group-name',
                    'Values': [GroupName]
                },
                {
                    'Name': 'vpc-id',
                    'Values': [VpcId]
                }
            ],
            DryRun=DryRun
        )
        GroupId = response['SecurityGroups'][0]

    return GroupId