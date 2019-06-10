import json, os

with open(os.path.join(os.path.dirname(os.path.abspath(__file__)), '../resource/privateSubnet.json')) as inf:
    privateSubnet = json.loads(inf.read())


    # json_default subnet Arn
    arnPrivateSubnet = privateSubnet['SubnetArn']

    # availability zone for subnet
    azPrivateSubnet = privateSubnet['AvailabilityZone']

    # json_default subnet Id
    privateSubnetId = privateSubnet['SubnetId']

with open(os.path.join(os.path.dirname(os.path.abspath(__file__)), '../resource/privateRouteTable.json')) as inf:
    privateRouteTable = json.loads(inf.read())
    privateRouteTableId = privateRouteTable['RouteTableId']