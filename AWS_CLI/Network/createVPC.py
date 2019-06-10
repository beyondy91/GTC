# -*- coding: utf-8 -*-
from Default.defaults import *

def createVPC(client,
              CidrBlock,
              AmazonProvidedIpv6CidrBlock=True,
              DryRun=False,
              InstanceTenancy='json_default',
              name='ds-pipeline-vpc'):
    try:
        return client.describe_vpcs(Filters=[
            {
                'Name': 'tag:Name',
                'Values': [
                    name,
                ]
            },
        ])['Vpcs'][0]

    except:
        print('Failed to search %s vpc' % (name))
        print(sys.exc_info())
        response = client.create_vpc(
            CidrBlock=CidrBlock,
            AmazonProvidedIpv6CidrBlock=AmazonProvidedIpv6CidrBlock,
            DryRun=DryRun,
            InstanceTenancy=InstanceTenancy
        )
        VpcId = response['Vpc']

        client.create_tags(
            Resources=[
                VpcId,
            ],
            Tags=[
                  {
                      'Key': 'Name',
                      'Value': name,
                  }
              ]
        )
        return VpcId