# -*- coding: utf-8 -*-
from Default.defaults import *

def updateLambdaFunction(client,
                         FunctionName,
                         Code,
                         Publish,
                         DryRun):
    try:
        kwargs = Code
        kwargs['FunctionName'] = FunctionName
        kwargs['Publish'] = Publish
        kwargs['DryRun'] = DryRun
        client.update_function_code(
            **kwargs
        )
    except:
        print('Failed to update %s lambda function'%(FunctionName))
        print(sys.exc_info())