from Default.defaults import *


def createUsagePlanKey(client,
                       usagePlanId,
                       keyId):

    try:
        planKey = client.get_usage_plan_key(
            usagePlanId=usagePlanId,
            keyId=keyId
        )
    except:
        planKey = client.create_usage_plan_key(
            usagePlanId=usagePlanId,
            keyId=keyId,
            keyType='API_KEY'
        )
    return planKey
