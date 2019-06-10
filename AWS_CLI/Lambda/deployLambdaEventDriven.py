# -*- coding: utf-8 -*-

from Default.defaults import *
from .deployLambdaFunction import deployLambdaFunction
from .getLambdaFunctionArn import getLambdaFunctionArn
from .addLambdaInvokePermission import addLambdaInvokePermission
from S3.createBucketNotification import createBucketNotification


def deployLambdaEventDriven(client,
                            Role,
                            dir,
                            SecurityGroupIds,
                            SubnetIds,
                            env,
                            deployConfig,
                            functionName,
                            sourceName,
                            sourceService,
                            sourceArn,
                            FilterRules=None):
    deployLambdaFunction(Role=Role,
                         dir=dir,
                         SecurityGroupIds=SecurityGroupIds,
                         SubnetIds=SubnetIds,
                         env=env,
                         deployConfig=deployConfig,
                         functionName=functionName)
    arnLambda = getLambdaFunctionArn(client=client,
                                     FunctionName=functionName)

    # lambda를 invoke할 수 있는 권한을 S3 bucket에 부여해야 됨
    addLambdaInvokePermission(client=client,
                              FunctionName=functionName,
                              StatementId='%s_invoke_%s' % (sourceName, functionName),
                              Action='lambda:InvokeFunction',
                              Principal='%s.amazonaws.com' % (sourceService),
                              SourceArn=sourceArn)
    if sourceService == 's3':
        # 권한 부여 후에 create 이벤트로 람다 트리거
        createBucketNotification(
            client=clientS3,
            Bucket=sourceName,
            NotificationConfiguration={
                'LambdaFunctionConfigurations': [
                    {
                        'Id': '%s_upload_trigger_lambda' % (sourceName),
                        'LambdaFunctionArn': arnLambda,
                        'Events': ['s3:ObjectCreated:*'],
                        'Filter': {
                            'Key': {
                                'FilterRules': FilterRules
                            }
                        }
                    }
                ]
            }
        )
    elif sourceService == 'dynamodb':
        for stream in clientDynamoDBStreams.list_streams(TableName=sourceName)['Streams']:
            arnStream = stream['StreamArn']
            try:
                client.create_event_source_mapping(
                    EventSourceArn=arnStream,
                    FunctionName=functionName,
                    Enabled=True,
                    StartingPosition='LATEST'
                )
            except:
                print(sys.exc_info())