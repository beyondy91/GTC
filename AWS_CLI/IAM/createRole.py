# -*- coding: utf-8 -*-
from Default.defaults import *

def createRole(client,
               RoleName,
               assume_role_file,
               Description,
               Path):
    try:
        with open(assume_role_file) as assume_role_file:
            role_policy = json.loads(assume_role_file.read())
            role_policy['Statement'][0]['Principal']['AWS'] = rootArn
            response = client.create_role(
                RoleName=RoleName,
                AssumeRolePolicyDocument=json.dumps(role_policy),
                Description=Description,
                Path=Path
            )
            arn = response['Role']
    except:
        print('Failed to create %s role'%(RoleName))
        print(sys.exc_info())
        for role in client.list_roles()['Roles']:
            if role['RoleName'] == RoleName:
                arn = role
    return arn