from Default.defaults import *


def createUsagePlan(client,
                    planName,
                    restAPIId,
                    stageName,
                    targetPath='',
                    httpMethod='POST'):
    try:
        plan = list(filter(lambda x: x['name'] == planName and
                                     x['apiStages'][0]['apiId'] == restAPIId and
                                     x['apiStages'][0]['stage'] == stageName,
                           clientAPI.get_usage_plans(
                               limit=500
                           )['items']))[0]
    except:
        plan = client.create_usage_plan(
            name=planName,
            apiStages=[
                {
                    'apiId': restAPIId,
                    'stage': stageName,
                    'throttle': {
                        '{0}/{1}'.format(targetPath, httpMethod): {
                            'burstLimit': 100,
                            'rateLimit': 100
                        }
                    }
                },
            ],
            throttle={
                'burstLimit': 100,
                'rateLimit': 100
            },
            quota={
                'limit': 100000,
                'offset': 0,
                'period': 'DAY'
            }
        )
    return plan
