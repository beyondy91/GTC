# -*- coding: utf-8 -*-
from Default.defaults import *


def createDynamoTable(client,
                      AttributeDefinitions,
                      TableName,
                      KeySchema,
                      BillingMode='PAY_PER_REQUEST',
                      StreamEnabled=True,
                      StreamViewType='NEW_AND_OLD_IMAGES'):
    try:
        response = client.create_table(
            AttributeDefinitions=AttributeDefinitions,
            TableName=TableName,
            KeySchema=KeySchema,
            BillingMode=BillingMode,
            StreamSpecification={
                'StreamEnabled': StreamEnabled,
                'StreamViewType': StreamViewType}, )['TableDescription']
    except:
        print('Failed to create %s table' % (TableName))
        print(sys.exc_info())
    while True:
        response = client.describe_table(TableName=TableName)['Table']
        if response['TableStatus'] == 'ACTIVE':
            break
    return response
