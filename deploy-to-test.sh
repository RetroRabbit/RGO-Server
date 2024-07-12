#!/bin/bash

# Variables
TEST_SERVER_USER=$TEST_SERVER_USER
TEST_SERVER_IP=$TEST_SERVER_IP
TARGET_DIRECTORY=$TARGET_DIRECTORY
ARTIFACT_NAME=$ARTIFACT_NAME
ARTIFACT_PATH="$BUILD_ARTIFACTSTAGINGDIRECTORY/$ARTIFACT_NAME.zip"

# Step 1: Copy the build artifacts to the test server
echo "Copying build artifacts to the test server..."
scp $ARTIFACT_PATH $TEST_SERVER_USER@$TEST_SERVER_IP:$TARGET_DIRECTORY

# Step 2: Connect to the test server and deploy
echo "Deploying application on the test server..."
ssh $TEST_SERVER_USER@$TEST_SERVER_IP << 'ENDSSH'
  # Navigate to the deployment directory
  cd $TARGET_DIRECTORY

  # Unzip the artifacts
  unzip -o $ARTIFACT_NAME.zip

  # Stop the existing application (if running)
  systemctl stop your-application.service

  # Deploy the new version
  cp -R $ARTIFACT_NAME/* /path/to/your/application/directory

  # Start the application
  systemctl start your-application.service

  # Clean up
  rm -rf $ARTIFACT_NAME
  rm $ARTIFACT_NAME.zip
ENDSSH

echo "Deployment to test server completed."
