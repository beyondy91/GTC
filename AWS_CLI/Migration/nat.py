# -*- coding: utf-8 -*-
from __future__ import print_function
import sys, os
sys.path.append(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))
os.chdir(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))

from Default.defaults import *

from Network.createNATGateway import createNATGateWay
from Network.createRoute import createRoute
from loadVPC import *
from loadPrivateSubnet import *
from loadPublicSubnet import *
from json_default import json_default

# NAT Gateway 생성
nat = createNATGateWay(client=clientEC2,
                       VpcId=vpcId,
                       SubnetId=publicSubnetId)
natId = nat['NatGatewayId']

# NAT를 private subnet에 연결
createRoute(client=clientEC2,
            DestinationCidrBlock='0.0.0.0/0',
            NatGatewayId=natId,
            RouteTableId=privateRouteTableId)

with open('resource/nat.json', 'w') as outf:
    outf.write(json.dumps(nat, default=json_default))