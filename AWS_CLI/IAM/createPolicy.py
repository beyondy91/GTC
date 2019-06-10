# -*- coding: utf-8 -*-
from Default.defaults import *

def createPolicy(client,
                 PolicyName,
                 Path,
                 policyFilename,
                 Description):
    try:
        with open(policyFilename) as policy_file:
            response = client.create_policy(
                PolicyName=PolicyName,
                Path=Path,
                PolicyDocument=policy_file.read(),
                Description=Description
            )
            arn = response['Policy']
    except:
        print('Failed to create %s policy'%(PolicyName))
        print(sys.exc_info())
        for policy in client.list_policies()['Policies']:
            if policy['PolicyName'] == PolicyName:
                arn = policy
    return arn