import json, os

with open(os.path.join(os.path.dirname(os.path.abspath(__file__)), '../resource/vpc.json')) as inf:
    vpc = json.loads(inf.read())
    vpcId = vpc['VpcId']