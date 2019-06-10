# -*- coding: utf-8 -*-
from Default.defaults import *

def putBucketEncryption(client,
                        Bucket,
                        SSEAlgorithm,
                        KeyId=None):
    ApplyServerSideEncryptionByDefault = dict(SSEAlgorithm=SSEAlgorithm)
    if KeyId is not None:
        ApplyServerSideEncryptionByDefault['KeyId'] = KeyId
    response = client.put_bucket_encryption(
        Bucket=Bucket,
        ServerSideEncryptionConfiguration={
            'Rules': [
                {
                    'ApplyServerSideEncryptionByDefault': ApplyServerSideEncryptionByDefault
                },
            ]
        }
    )