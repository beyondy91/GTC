# -*- coding: utf-8 -*-
from Default.defaults import *

def addLambdaInvokePermission(client,
                              FunctionName='string',
                              StatementId='string',
                              Action='lambda:InvokeFunction',
                              Principal='SourceArn',
                              SourceArn='string'):
    try:
        response = client.add_permission(
            FunctionName=FunctionName,
            StatementId=StatementId,
            Action=Action,
            Principal=Principal,
            SourceArn=SourceArn
        )
    except:
        print('Failed to create %s permission on %s lambda function'%(StatementId, FunctionName))
        print(sys.exc_info())