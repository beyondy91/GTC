# -*- coding: utf-8 -*-
from Default.defaults import *

def createLambdaMapping(client,
                        EventSourceArn='string',
                        FunctionName='string',
                        Enabled=True):
    try:
        response = client.create_event_source_mapping(
            EventSourceArn=EventSourceArn,
            FunctionName=FunctionName,
            Enabled=Enabled
        )
        return response['UUID']
    except:
        print('Failed to create %s-%s lambda function mapping'%(EventSourceArn, FunctionName))
        print(sys.exc_info())
        response = client.list_event_source_mappings(
            EventSourceArn=EventSourceArn,
            FunctionName=FunctionName
        )
        return response['UUID']