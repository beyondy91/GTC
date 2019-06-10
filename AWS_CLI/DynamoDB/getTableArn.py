# -*- coding: utf-8 -*-
from Default.defaults import *

def getTableArn(Table,
                Region=locationConstraint,
                AccountId=account):
    return 'arn:aws:dynamodb:%s:%s:table/%s'%(Region, AccountId, Table)