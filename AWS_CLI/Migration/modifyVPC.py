# -*- coding: utf-8 -*-
from __future__ import print_function
import sys, os
sys.path.append(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))
os.chdir(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))

from Default.defaults import *
from Network.modifyVPCAttribute import modifyVPCAttribute
from loadVPC import *

# DNS check activate
modifyVPCAttribute(client=clientEC2,
                   VpcId=vpcId,
                   EnableDnsHostnames=True,
                   EnableDnsSupport=True)