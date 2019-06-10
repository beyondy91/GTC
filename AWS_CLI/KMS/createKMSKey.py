# -*- coding: utf-8 -*-
from Default.defaults import *

def createKMSKey(client,
                 Description,
                 Tags=[{}]):
    response = client.create_key(
        Description=Description,
        Tags=Tags)
    return response['KeyMetadata']