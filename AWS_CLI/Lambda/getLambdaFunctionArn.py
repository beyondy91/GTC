# -*- coding: utf-8 -*-
from Default.defaults import *

def getLambdaFunctionArn(client,
                         FunctionName):
    return client.get_function(
        FunctionName=FunctionName
    )['Configuration']['FunctionArn']