import json, os

with open(os.path.join(os.path.dirname(os.path.abspath(__file__)), '../resource/api.json')) as inf:
    apiPreferences = json.loads(inf.read())