# -*- coding: utf-8 -*-
from __future__ import print_function
import sys, os
sys.path.append(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))
os.chdir(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))

from Default.defaults import *
from Network.createSubnet import createSubnet
from Network.createRouteTable import createRouteTable
from loadVPC import *

# json_default subnet 생성
privateSubnet = createSubnet(client=clientEC2,
                             CidrBlock=privateCidrBlock,
                             VpcId=vpcId,
                             DryRun=False,
                             name=privateSubnetName)


# json_default subnet Arn
arnPrivateSubnet = privateSubnet['SubnetArn']

# availability zone for subnet
azPrivateSubnet = privateSubnet['AvailabilityZone']

# json_default subnet Id
privateSubnetId = privateSubnet['SubnetId']

# json_default route table 생성
privateRouteTable = createRouteTable(client=clientEC2,
                                     VpcId=vpcId,
                                     Name=privateRouteTableName)
privateRouteTableId = privateRouteTable['RouteTableId']

try:
    clientEC2.associate_route_table(DryRun=False,
                                    RouteTableId=privateRouteTableId,
                                    SubnetId=privateSubnetId)
except:
    print(sys.exc_info())

with open('resource/privateSubnet.json', 'w') as outf:
    outf.write(json.dumps(privateSubnet))

with open('resource/privateRouteTable.json', 'w') as outf:
    outf.write(json.dumps(privateRouteTable))