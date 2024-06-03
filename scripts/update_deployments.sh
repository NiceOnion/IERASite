#!/bin/bash

set -e

# Define your deployments and container names
DEPLOYMENTS=("announcement-service" "gateway")
CONTAINERS=("ierasite" "ierasite")
IMAGE_ANNOUNCEMENT="niceonion/ierasite:announcements-latest"
IMAGE_GATEWAY="niceonion/ierasite:gateway-latest"

# Pull the latest images from DockerHub
docker pull $IMAGE_ANNOUNCEMENT
docker pull $IMAGE_GATEWAY

# Update each deployment with the new image
kubectl set image deployment/announcement-service ierasite=$IMAGE_ANNOUNCEMENT --record
kubectl set image deployment/gateway ierasite=$IMAGE_GATEWAY --record

# Optionally, remove old images to free up space
docker image prune -f
