# -*- coding: utf-8 -*-
from __future__ import print_function
import sys, os
sys.path.append(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))
os.chdir(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))

from Default.defaults import *
from Network.createVPC import createVPC


# json_default VPC 생성
vpc = createVPC(client=clientEC2,
                CidrBlock=privateCidrBlock,
                AmazonProvidedIpv6CidrBlock=True,
                DryRun=False,
                InstanceTenancy='json_default',
                name=vpcName)
with open('resource/vpc.json', 'w') as outf:
    outf.write(json.dumps(vpc))