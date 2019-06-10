# -*- coding: utf-8 -*-
from Default.defaults import *

def createRouteTable(client,
                  VpcId,
                  Name):
    try:
        response = client.describe_route_tables(
            Filters=[
                {
                    'Name': 'vpc-id',
                    'Values': [
                        VpcId,
                    ]
                },
                {
                    'Name': 'tag:Name',
                    'Values': [
                        Name,
                    ]
                },
            ],
            DryRun=False
        )
        return response['RouteTables'][0]
    except:
        print('Failed to get %s route table in %s vpc'%(Name, VpcId))
        print(sys.exc_info())
        response = client.create_route_table(
            DryRun=False,
            VpcId=VpcId
        )
        client.create_tags(
            DryRun=False,
            Resources=[response['RouteTable']['RouteTableId']],
            Tags=[
                {
                    'Key': 'Name',
                    'Value': Name
                },
            ]
        )
        return response['RouteTable']