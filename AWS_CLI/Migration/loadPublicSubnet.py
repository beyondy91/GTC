import json, os

with open(os.path.join(os.path.dirname(os.path.abspath(__file__)), '../resource/publicSubnet.json')) as inf:
    publicSubnet = json.loads(inf.read())


    # json_default subnet Arn
    arnpublicSubnet = publicSubnet['SubnetArn']

    # availability zone for subnet
    azpublicSubnet = publicSubnet['AvailabilityZone']

    # json_default subnet Id
    publicSubnetId = publicSubnet['SubnetId']

with open(os.path.join(os.path.dirname(os.path.abspath(__file__)), '../resource/publicRouteTable.json')) as inf:
    publicRouteTable = json.loads(inf.read())
    publicRouteTableId = publicRouteTable['RouteTableId']