#!/bin/bash

CLUSTER_NAME=BatchEcsNodeTestStack-ClusterEB0386A7-jonoMOHF7IiG

aws ecs describe-clusters  --cluster $CLUSTER_NAME --output yaml
CONTAINER_INSTANCES=$(aws ecs list-container-instances --cluster $CLUSTER_NAME --query "containerInstanceArns")

aws ecs describe-container-instances --container-instances $CONTAINER_INSTANCES --cluster $CLUSTER_NAME \
    --query "containerInstances[].{registeredResources:registeredResources[?name=='MEMORY'].integerValue | [0], InstanceType: attributes[?name=='ecs.instance-type'].value | [0]}"  --output text
    --output table
    --output yaml | yq e 
 