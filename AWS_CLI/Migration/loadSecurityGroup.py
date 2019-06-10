import json, os

with open(os.path.join(os.path.dirname(os.path.abspath(__file__)), '../resource/group.json')) as inf:
    group = json.loads(inf.read())
    groupId = group['GroupId']