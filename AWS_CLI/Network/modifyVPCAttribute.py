# -*- coding: utf-8 -*-
from Default.defaults import *

def modifyVPCAttribute(client,
                       VpcId,
                       EnableDnsHostnames=True,
                       EnableDnsSupport=True):
    client.modify_vpc_attribute(
        EnableDnsHostnames={
            'Value': EnableDnsHostnames
        },
        VpcId=VpcId)
    client.modify_vpc_attribute(
        EnableDnsSupport={
            'Value': EnableDnsSupport
        },
        VpcId=VpcId)
