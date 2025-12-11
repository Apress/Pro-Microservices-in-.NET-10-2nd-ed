#!/bin/sh

RESOURCE_GROUP="rg-pro-microservices"
LOCATION="canadacentral"
APP_SERVICE_PLAN="sp-pro-microservices"
APP_NAME="wa-pro-microservices"
CONTAINER_IMAGE="myaccount.azurecr.io/myimage:latest"
APIM_NAME="apim-pro-microservices"
SERVICE_PLAN_SKU="F1" # Free tier for testing

az group create --name $RESOURCE_GROUP --location $LOCATION

az apim create --name $APIM_NAME --resource-group $RESOURCE_GROUP --location $LOCATION --publisher-email "admin@example.com" --publisher-name "Admin" --sku-name Developer

az appservice plan create --name $APP_SERVICE_PLAN --resource-group $RESOURCE_GROUP --is-linux --sku $SERVICE_PLAN_SKU
az webapp create --name $APP_NAME --resource-group $RESOURCE_GROUP --plan $APP_SERVICE_PLAN --deployment-container-image-name $CONTAINER_IMAGE

APP_URL=$(az webapp show --name $APP_NAME --resource-group $RESOURCE_GROUP --query defaultHostName --output tsv)

az apim api import --resource-group myResourceGroup --service-name $APIM_NAME --path api/customer --api-id $APP_NAME --specification-format OpenApi --specification-url "https://{$APP_URL}/swagger/v1/swagger.json" --display-name "Customer" --protocols https
