# -*- coding: utf-8 -*-
from Default.defaults import *

def attachPolicies(client,
                   RoleName,
                   Policies):
    [client.attach_role_policy(
        PolicyArn=policy,
        RoleName=RoleName) for policy in Policies]