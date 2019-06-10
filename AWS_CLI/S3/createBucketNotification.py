# -*- coding: utf-8 -*-
from Default.defaults import *

def createBucketNotification(client,
                             Bucket,
                             NotificationConfiguration):
    try:
        response = client.put_bucket_notification_configuration(
            Bucket=Bucket,
            NotificationConfiguration=NotificationConfiguration
        )
    except:
        print('Failed to create %s notification'%(Bucket))
        print(sys.exc_info())