import json, os

with open(os.path.join(os.path.dirname(os.path.abspath(__file__)), '../resource/arnRole.json')) as inf:
    arnRoles = json.loads(inf.read())