# -*- coding: utf-8 -*-
from __future__ import print_function
import sys, os
sys.path.append(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))
os.chdir(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))

from Default.defaults import *

from Network.createSecurityGroup import createSecurityGroup
from loadVPC import *
from json_default import json_default

# security group 생성
group = createSecurityGroup(client=clientEC2,
                            Description='Security group for DS pipeline',
                            GroupName=securityGroupName,
                            VpcId=vpcId,
                            DryRun=False)
groupId = group['GroupId']

with open('resource/group.json', 'w') as outf:
    outf.write(json.dumps(group, default=json_default))