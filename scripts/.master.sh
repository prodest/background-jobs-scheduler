#!/bin/bash

export RANCHER_STACK=prd

# deploy BackgroundJobsScheduler
export RANCHER_SERVICE=background-jobs-scheduler
export IMAGE_NAME=$DOCKER_IMAGE:$DOCKER_TAG
. ./scripts/.deploy.sh