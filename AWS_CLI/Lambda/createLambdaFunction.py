# -*- coding: utf-8 -*-
from Default.defaults import *

def createLambdaFunction(client,
                         FunctionName,
                         Runtime='dotnetcore2.1',
                         Role='string',
                         Handler='string',
                         Code={},
                         Description='string',
                         Timeout=900,
                         MemorySize=128,
                         Publish=False,
                         VpcConfig={},
                         DeadLetterConfig={},
                         Environment={},
                         KMSKeyArn='',
                         TracingConfig={},
                         Tags={},
                         Layers=[]):
    try:
        response = client.create_function(
            FunctionName=FunctionName,
            Runtime=Runtime,
            Role=Role,
            Handler=Handler,
            Code=Code,
            Description=Description,
            Timeout=Timeout,
            MemorySize=MemorySize,
            Publish=Publish,
            VpcConfig=VpcConfig,
            DeadLetterConfig=DeadLetterConfig,
            Environment=Environment,
            TracingConfig=TracingConfig,
            Tags=Tags,
            Layers=Layers)
        arn = response['FunctionArn']
    except:
        print('Failed to create %s lambda function'%(FunctionName))
        print(sys.exc_info())
        response = client.get_function(
            FunctionName=FunctionName
        )
        arn = response['Configuration']['FunctionArn']

    return arn