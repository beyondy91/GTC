# -*- coding: utf-8 -*-
from Default.defaults import *

def putBucketFullAccessPolicy(resource,
                              BucketName,
                              RoleArn,
                              PolicyFile):
    with open(PolicyFile) as default_file:
        default_policy = json.loads(default_file.read())
        default_policy['Statement'][0]['Principal']['AWS'] = RoleArn
        default_policy['Statement'][0]['Resource'] = ["arn:aws:s3:::%s"%(BucketName), "arn:aws:s3:::%s/*"%(BucketName)]
        bucket_policy = resource.BucketPolicy(BucketName)
        bucket_policy.put(Policy=json.dumps(default_policy))