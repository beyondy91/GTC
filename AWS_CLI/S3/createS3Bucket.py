# -*- coding: utf-8 -*-
from Default.defaults import *

def createS3Bucket(client,
                   ACL='private',
                   Bucket='',
                   CreateBucketConfiguration={'LocationConstraint': 'ap-northeast-1'},
                   ObjectLockEnabledForBucket=True):
    try:
        response = clientS3.create_bucket(
            ACL=ACL,
            Bucket=Bucket,
            CreateBucketConfiguration=CreateBucketConfiguration,
            ObjectLockEnabledForBucket=ObjectLockEnabledForBucket
        )
    except:
        print('Failed to create %s bucket'%(Bucket))
        print(sys.exc_info())