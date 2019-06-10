import json, os

with open(os.path.join(os.path.dirname(os.path.abspath(__file__)), '../resource/igw.json')) as inf:
    igw = json.loads(inf.read())
    igwId = igw['InternetGatewayId']