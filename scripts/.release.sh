#!/bin/bash

export RANCHER_STACK=hmg

# deploy BackgroundJobsScheduler
export RANCHER_SERVICE=background-jobs-scheduler
export IMAGE_NAME=$DOCKER_IMAGE-hmg:$DOCKER_TAG
. ./scripts/.deploy.sh
