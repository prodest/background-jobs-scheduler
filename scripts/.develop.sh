#!/bin/bash

export RANCHER_STACK=dev

# deploy WebAPI
export RANCHER_SERVICE=background-jobs-scheduler
export IMAGE_NAME=$DOCKER_IMAGE-dev:$DOCKER_TAG
. ./scripts/.deploy.sh
