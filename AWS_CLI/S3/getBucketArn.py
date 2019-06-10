# -*- coding: utf-8 -*-
from Default.defaults import *

def getBucketArn(Bucket):
    return 'arn:aws:s3:::%s'%(Bucket)