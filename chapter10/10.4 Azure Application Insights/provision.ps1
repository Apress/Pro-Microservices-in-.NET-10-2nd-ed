
RESOURCE_GROUP = "rg-pro-microservices"
APP_NAME = "wa-pro-microservices"
PLAN_NAME = "sp-pro-microservices"
LOCATION = "canadacentral"
CONTAINER_IMAGE = "myregistry.azurecr.io/myapp:latest"
APPINSIGHTS_NAME = "$APP_NAME-ai"
SERVICE_PLAN_SKU = "F1" # Free tier for testing

az group create --name $RESOURCE_GROUP --location $LOCATION
az appservice plan create --name $PLAN_NAME --resource-group $RESOURCE_GROUP --is-linux --sku $SERVICE_PLAN_SKU
az webapp create --name $APP_NAME --resource-group $RESOURCE_GROUP --plan $PLAN_NAME --deployment-container-image-name $CONTAINER_IMAGE
az monitor app-insights component create --app "$APPINSIGHTS_NAME" --location $LOCATION --resource-group $RESOURCE_GROUP --application-type web
INSTRUMENTATION_KEY=$(az monitor app-insights component show --app "$APPINSIGHTS_NAME" --resource-group $RESOURCE_GROUP --query "instrumentationKey" --output tsv)
CONNECTION_STRING=$(az monitor app-insights component show --app "$APPINSIGHTS_NAME" --resource-group $RESOURCE_GROUP --query "connectionString" --output tsv)
az webapp config appsettings set --name $APP_NAME --resource-group $RESOURCE_GROUP --settings APPINSIGHTS_INSTRUMENTATIONKEY=$INSTRUMENTATION_KEY APPLICATIONINSIGHTS_CONNECTION_STRING="$CONNECTION_STRING" ApplicationInsightsAgent_EXTENSION_VERSION=~3
az webapp log config --name $APP_NAME --resource-group $RESOURCE_GROUP --docker-container-logging filesystem

echo "App URL: https://$APP_NAME.azurewebsites.net"
# stream logs to the console:
az webapp log tail --name $APP_NAME --resource-group $RESOURCE_GROUP
