import datetime
import json

def json_default(o):
    if isinstance(o, (datetime.date, datetime.datetime)):
        return o.isoformat()