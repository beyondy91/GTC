# -*- coding: utf-8 -*-
from Default.defaults import *

def deployLambdaFunction(Role,
                         dir,
                         SecurityGroupIds,
                         SubnetIds,
                         env=DefaultPreference,
                         deployConfig='aws-lambda-tools-defaults.json',
                         functionName=None):
    original_dir = os.getcwd()
    os.chdir(os.path.join(os.path.dirname(os.path.abspath(__file__)), dir))

    with codecs.open(deployConfig, 'r', encoding='utf-8-sig') as deploy_config_file:
        deploy_config_file_content = deploy_config_file.read()
        print(deploy_config_file_content)
        config = json.loads(deploy_config_file_content)
    config['function-role'] = Role
    config['function-subnets'] = SubnetIds
    config['function-security-groups'] = SecurityGroupIds
    config['environment-variables'] = env
    if functionName != None:
        config['function-name'] = functionName
    with codecs.open(deployConfig, 'w', encoding='utf-8-sig') as deploy_config_file:
        json.dump(config, deploy_config_file, indent=2)
    os.system('dotnet lambda deploy-function -cfg %s'%deployConfig)

    os.chdir(original_dir)

    return config['function-name']