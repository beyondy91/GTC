# -*- coding: utf-8 -*-
from __future__ import print_function
import sys, os
sys.path.append(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))
os.chdir(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))

from Default.defaults import *
from Network.createInternetGateway import createInternetGateway
from Network.createRoute import createRoute
from loadVPC import *
from loadPublicSubnet import *

os.chdir(os.path.dirname(os.path.abspath(__file__)))

# internet gateway 생성
igw = createInternetGateway(client=clientEC2,
                            DryRun=False,
                            VpcId=vpcId)
igwId = igw['InternetGatewayId']

# IGW를 public subnet에 연결
createRoute(client=clientEC2,
            DestinationCidrBlock='0.0.0.0/0',
            GatewayId=igwId,
            RouteTableId=publicRouteTableId)

with open('resource/igw.json', 'w') as outf:
    outf.write(json.dumps(igw))