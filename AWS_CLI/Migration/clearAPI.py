from __future__ import print_function
import sys, os

sys.path.append(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))
os.chdir(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))

from Default.defaults import *
import time

for api in clientAPI.get_rest_apis(limit=500)['items']:
    clientAPI.delete_rest_api(id=api['id'])
    time.sleep(61)

for plan in clientAPI.get_usage_plans(limit=500)['items']:
    for key in clientAPI.get_usage_plan_keys(usagePlanId=plan['id'])['items']:
        clientAPI.delete_usage_plan_key(usagePlanId=plan['id'], keyId=key['id'])
    clientAPI.delete_usage_plan(usagePlanId=plan['id'])

for key in clientAPI.get_api_keys(limit=500)['items']:
    clientAPI.delete_api_key(apiKey=key['id'])