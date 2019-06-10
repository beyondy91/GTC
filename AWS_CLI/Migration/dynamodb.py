# -*- coding: utf-8 -*-
from __future__ import print_function
import sys, os

sys.path.append(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))
os.chdir(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))

from Default.defaults import *
from DynamoDB.createDynamoTable import createDynamoTable

# preference 저장용 DynamoDB 테이블
preferenceTable = createDynamoTable(client=clientDynamoDB,
                                    AttributeDefinitions=[
                                        {
                                            'AttributeName': 'key',
                                            'AttributeType': 'S'
                                        }
                                    ],
                                    TableName=tablePreference,
                                    KeySchema=[
                                        {
                                            'AttributeName': 'key',
                                            'KeyType': 'HASH'
                                        }
                                    ])

# progress 저장용 DynamoDB 테이블
progressTable = createDynamoTable(client=clientDynamoDB,
                                  AttributeDefinitions=[
                                      {
                                          'AttributeName': 'sn',
                                          'AttributeType': 'S'
                                      },
                                      {
                                          'AttributeName': 'stage',
                                          'AttributeType': 'S'
                                      }
                                  ],
                                  TableName=tableProgress,
                                  KeySchema=[
                                      {
                                          'AttributeName': 'sn',
                                          'KeyType': 'HASH'
                                      },
                                      {
                                          'AttributeName': 'stage',
                                          'KeyType': 'RANGE'
                                      }
                                  ])

# filename mapping 저장용 DynamoDB 테이블
filenameTable = createDynamoTable(client=clientDynamoDB,
                                  AttributeDefinitions=[
                                      {
                                          'AttributeName': 'sn',
                                          'AttributeType': 'S'
                                      },
                                      {
                                          'AttributeName': 'filename',
                                          'AttributeType': 'S'
                                      }
                                  ],
                                  TableName=tableFilename,
                                  KeySchema=[
                                      {
                                          'AttributeName': 'sn',
                                          'KeyType': 'HASH'
                                      },
                                      {
                                          'AttributeName': 'filename',
                                          'KeyType': 'RANGE'
                                      }
                                  ])

# manifest filename mapping 저장용 DynamoDB 테이블
manifestTable = createDynamoTable(client=clientDynamoDB,
                                  AttributeDefinitions=[
                                      {
                                          'AttributeName': 'bpm',
                                          'AttributeType': 'S'
                                      }
                                  ],
                                  TableName=tableManifest,
                                  KeySchema=[
                                      {
                                          'AttributeName': 'bpm',
                                          'KeyType': 'HASH'
                                      }
                                  ])

table = resourceDynamoDB.Table(tableManifest)
table.put_item(
    Item={
        'bpm': 'Genoplan_beta_20022517X355438_A1.bpm',
        'manifest': 'Genoplan_beta_grch37'
    }
)

# Genotype meta 저장용 DynamoDB 테이블
metaGenotypeTable = createDynamoTable(client=clientDynamoDB,
                                      AttributeDefinitions=[
                                          {
                                              'AttributeName': 'code',
                                              'AttributeType': 'S'
                                          },
                                          {
                                              'AttributeName': 'var',
                                              'AttributeType': 'S'
                                          }
                                      ],
                                      TableName=tableMetaGenotype,
                                      KeySchema=[
                                          {
                                              'AttributeName': 'code',
                                              'KeyType': 'HASH'
                                          },
                                          {
                                              'AttributeName': 'var',
                                              'KeyType': 'RANGE'
                                          }
                                      ])

# Survey meta 저장용 DynamoDB 테이블
metaSurveyTable = createDynamoTable(client=clientDynamoDB,
                                    AttributeDefinitions=[
                                        {
                                            'AttributeName': 'code',
                                            'AttributeType': 'S'
                                        },
                                        {
                                            'AttributeName': 'var',
                                            'AttributeType': 'S'
                                        }
                                    ],
                                    TableName=tableMetaSurvey,
                                    KeySchema=[
                                        {
                                            'AttributeName': 'code',
                                            'KeyType': 'HASH'
                                        },
                                        {
                                            'AttributeName': 'var',
                                            'KeyType': 'RANGE'
                                        }
                                    ])

# trait name, code 등 metadata 저장용 DynamoDB 테이블
metaTable = createDynamoTable(client=clientDynamoDB,
                              AttributeDefinitions=[
                                  {
                                      'AttributeName': 'code',
                                      'AttributeType': 'S'
                                  }
                              ],
                              TableName=tableMeta,
                              KeySchema=[
                                  {
                                      'AttributeName': 'code',
                                      'KeyType': 'HASH'
                                  }
                              ])

# Result 저장용 DynamoDB 테이블
resultTable = createDynamoTable(client=clientDynamoDB,
                                AttributeDefinitions=[
                                    {
                                        'AttributeName': 'sn',
                                        'AttributeType': 'S'
                                    },
                                    {
                                        'AttributeName': 'code',
                                        'AttributeType': 'S'
                                    }
                                ],
                                TableName=tableResult,
                                KeySchema=[
                                    {
                                        'AttributeName': 'sn',
                                        'KeyType': 'HASH'
                                    },
                                    {
                                        'AttributeName': 'code',
                                        'KeyType': 'RANGE'
                                    }
                                ])

# simulation progress 저장용 DynamoDB 테이블
simulationTable = createDynamoTable(client=clientDynamoDB,
                                    AttributeDefinitions=[
                                        {
                                            'AttributeName': 'sn',
                                            'AttributeType': 'S'
                                        },
                                        {
                                            'AttributeName': 'code',
                                            'AttributeType': 'S'
                                        }
                                    ],
                                    TableName=tableSimulation,
                                    KeySchema=[
                                        {
                                            'AttributeName': 'sn',
                                            'KeyType': 'HASH'
                                        },
                                        {
                                            'AttributeName': 'code',
                                            'KeyType': 'RANGE'
                                        }
                                    ])

table = resourceDynamoDB.Table(tablePreference)
for preference in preferences:
    table.put_item(Item=dict(
        key=preference['key'],
        preference=json.dumps(preference['preference'])
    ))
