#!/bin/bash
set -e

export RANCHER_ENV=processo-eletronico
export RANCHER_START_FIRST=true
export DOCKER_TAG=${TRAVIS_COMMIT:0:7}

# publish BackgroundJobsScheduler
export DOCKER_IMAGE=prodest/background-jobs-scheduler

cd BackgroundJobsScheduler/BackgroundJobsScheduler
dotnet restore && dotnet publish -c release -r debian.8-x64 -o publish ./

cd ../../

docker build -t $DOCKER_IMAGE -f ./Dockerfile .
