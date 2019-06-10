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
publicSubnet = createSubnet(client=clientEC2,
                            CidrBlock=publicCidrBlock,
                            VpcId=vpcId,
                            DryRun=False,
                            name=publicSubnetName)

# json_default subnet Arn
arnpublicSubnet = publicSubnet['SubnetArn']

# availability zone for subnet
azpublicSubnet = publicSubnet['AvailabilityZone']

# json_default subnet Id
publicSubnetId = publicSubnet['SubnetId']

# json_default route table 생성
publicRouteTable = createRouteTable(client=clientEC2,
                                    VpcId=vpcId,
                                    Name=publicRouteTableName)
publicRouteTableId = publicRouteTable['RouteTableId']

try:
    clientEC2.associate_route_table(DryRun=False,
                                    RouteTableId=publicRouteTableId,
                                    SubnetId=publicSubnetId)
except:
    print(sys.exc_info())

with open('resource/publicSubnet.json', 'w') as outf:
    outf.write(json.dumps(publicSubnet))

with open('resource/publicRouteTable.json', 'w') as outf:
    outf.write(json.dumps(publicRouteTable))
