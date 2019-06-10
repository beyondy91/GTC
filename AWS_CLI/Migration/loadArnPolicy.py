import json, os

with open(os.path.join(os.path.dirname(os.path.abspath(__file__)), '../resource/arnPolicy.json')) as inf:
    arnPolicies = json.loads(inf.read())