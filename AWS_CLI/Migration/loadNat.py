import json, os

with open(os.path.join(os.path.dirname(os.path.abspath(__file__)), '../resource/nat.json')) as inf:
    nat = json.loads(inf.read())
    natId = nat['NatGatewayId']