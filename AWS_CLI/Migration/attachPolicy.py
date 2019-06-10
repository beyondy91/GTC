# -*- coding: utf-8 -*-
from __future__ import print_function
import sys, os

sys.path.append(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))
os.chdir(os.path.join(os.path.dirname(os.path.abspath(__file__)), os.pardir))

from Default.defaults import *
from IAM.attachPolicies import attachPolicies
from loadArnPolicy import *
from loadArnRole import *

# LambdaS3GTCPolicyToPlink에 필요한 정책 연결
attachPolicies(client=clientIAM,
               RoleName='LambdaS3GTCToPlink',
               Policies=[arnPolicies['LambdaReadS3GTCPolicy'],
                         arnPolicies['LambdaReadS3BPMPolicy'],
                         arnPolicies['LambdaRWS3PlinkPolicy'],
                         arnPolicies['LambdaCreateNetworkInterface'],
                         arnPolicies['LambdaReadg1preferencePolicy'],
                         arnPolicies['LambdaReadg1manifestPolicy'],
                         arnPolicies['LambdaRWg1progressPolicy'],
                         arnPolicies['LambdaRWg1filenamePolicy'],
                         arnPolicies['LambdaLog']])

# LambdaS3BPMPolicyToBIM에 필요한 정책 연결
attachPolicies(client=clientIAM,
               RoleName='LambdaS3BPMToBIM',
               Policies=[arnPolicies['LambdaRWS3BPMPolicy'],
                         arnPolicies['LambdaCreateNetworkInterface'],
                         arnPolicies['LambdaReadg1preferencePolicy'],
                         arnPolicies['LambdaReadg1manifestPolicy'],
                         arnPolicies['LambdaRWg1progressPolicy'],
                         arnPolicies['LambdaLog']])

# LambdaSimulation에 필요한 정책 연결
attachPolicies(client=clientIAM,
               RoleName='LambdaSimulation',
               Policies=[arnPolicies['LambdaCreateNetworkInterface'],
                         arnPolicies['LambdaRWg1progressPolicy'],
                         arnPolicies['LambdaLog'],
                         arnPolicies['LambdaRWg1simulationPolicy'],
                         arnPolicies['LambdaRWS3SimulationPolicy'],
                         arnPolicies['LambdaReadg1manifestPolicy'],
                         arnPolicies['LambdaReadg1preferencePolicy']])

# LambdaSurveySimulation 필요한 정책 연결
attachPolicies(client=clientIAM,
               RoleName='LambdaSurveySimulation',
               Policies=[arnPolicies['LambdaCreateNetworkInterface'],
                         arnPolicies['LambdaRWg1progressPolicy'],
                         arnPolicies['LambdaLog'],
                         arnPolicies['LambdaRWg1simulationPolicy'],
                         arnPolicies['LambdaReadg1manifestPolicy'],
                         arnPolicies['LambdaReadg1preferencePolicy'],
                         arnPolicies['LambdaReadg1metasurveyPolicy']])

# LambdaLiftOver 필요한 정책 연결
attachPolicies(client=clientIAM,
               RoleName='LambdaLiftOver',
               Policies=[arnPolicies['LambdaCreateNetworkInterface'],
                         arnPolicies['LambdaRWg1progressPolicy'],
                         arnPolicies['LambdaLog'],
                         arnPolicies['LambdaReadg1manifestPolicy'],
                         arnPolicies['LambdaReadg1preferencePolicy'],
                         arnPolicies['LambdaRWS3BPMPolicy'],
                         arnPolicies['LambdaReadS3chainPolicy']])

# LambdaGenotypeSimulation 필요한 정책 연결
attachPolicies(client=clientIAM,
               RoleName='LambdaGenotypeSimulation',
               Policies=[arnPolicies['LambdaCreateNetworkInterface'],
                         arnPolicies['LambdaRWg1progressPolicy'],
                         arnPolicies['LambdaLog'],
                         arnPolicies['LambdaReadg1manifestPolicy'],
                         arnPolicies['LambdaReadg1preferencePolicy'],
                         arnPolicies['LambdaReadg1metagenotypePolicy']])

# LambdaListTrait 필요한 정책 연결
attachPolicies(client=clientIAM,
               RoleName='LambdaListTrait',
               Policies=[arnPolicies['LambdaCreateNetworkInterface'],
                         arnPolicies['LambdaRWg1progressPolicy'],
                         arnPolicies['LambdaLog'],
                         arnPolicies['LambdaReadg1manifestPolicy'],
                         arnPolicies['LambdaReadg1preferencePolicy'],
                         arnPolicies['LambdaReadg1metaPolicy']])

# LambdaCalcGenoRisk 필요한 정책 연결
attachPolicies(client=clientIAM,
               RoleName='LambdaCalcGenoRisk',
               Policies=[arnPolicies['LambdaCreateNetworkInterface'],
                         arnPolicies['LambdaRWg1progressPolicy'],
                         arnPolicies['LambdaLog'],
                         arnPolicies['LambdaReadg1manifestPolicy'],
                         arnPolicies['LambdaReadg1preferencePolicy'],
                         arnPolicies['LambdaRWg1resultPolicy'],
                         arnPolicies['LambdaReadS3PlinkPolicy'],
                         arnPolicies['LambdaReadS3BPMPolicy'],
                         arnPolicies['LambdaReadg1metagenotypePolicy']])

# LambdaGenoRiskAfterSimulation 필요한 정책 연결
attachPolicies(client=clientIAM,
               RoleName='LambdaGenoRiskAfterSimulation',
               Policies=[arnPolicies['LambdaCreateNetworkInterface'],
                         arnPolicies['LambdaRWg1progressPolicy'],
                         arnPolicies['LambdaLog'],
                         arnPolicies['LambdaReadg1manifestPolicy'],
                         arnPolicies['LambdaReadg1preferencePolicy'],
                         arnPolicies['LambdaReadg1simulationPolicy'],
                         arnPolicies['LambdaRWg1resultPolicy'],
                         arnPolicies['LambdaReadS3SimulationPolicy']])

# LambdaGTCSNChange 필요한 정책 연결
attachPolicies(client=clientIAM,
               RoleName='LambdaGTCSNChange',
               Policies=[arnPolicies['LambdaCreateNetworkInterface'],
                         arnPolicies['LambdaRWg1progressPolicy'],
                         arnPolicies['LambdaLog'],
                         arnPolicies['LambdaReadg1manifestPolicy'],
                         arnPolicies['LambdaReadg1preferencePolicy'],
                         arnPolicies['LambdaRWS3GTCPolicy']])
