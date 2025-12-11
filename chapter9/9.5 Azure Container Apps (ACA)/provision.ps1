
# Variables - customize these
RESOURCE_GROUP = "pro-microservices"
LOCATION = "canadacentral"
ACR_NAME = "my-registry"  # ACR name only (no .azurecr.io)
CONTAINERAPP_ENV = "pro-microservices"
CONTAINERAPP_NAME = "my-container"
IMAGE_NAME = "myimage:v0.1"
IDENTITY_NAME = "${CONTAINERAPP_NAME}-identity"
TARGET_PORT = 8080
SETTING = "ASPNETCORE_ENVIRONMENT = Production"

# if not already, `az login` and `az account set --subscription "..."`
az group create --name $RESOURCE_GROUP --location $LOCATION
az containerapp env create --name $CONTAINERAPP_ENV --resource-group $RESOURCE_GROUP --location $LOCATION
az identity create --name $IDENTITY_NAME --resource-group $RESOURCE_GROUP --location $LOCATION
PRINCIPAL_ID = $(az identity show --name $IDENTITY_NAME --resource-group $RESOURCE_GROUP --query 'principalId' -o tsv)
az role assignment create --assignee $PRINCIPAL_ID --role "AcrPull" --scope $(az acr show --name $ACR_NAME --query id -o tsv)
az containerapp create --name $CONTAINERAPP_NAME --resource-group $RESOURCE_GROUP --environment $CONTAINERAPP_ENV --image "$ACR_NAME.azurecr.io/$IMAGE_NAME" --target-port $TARGET_PORT --ingress 'external' --registry-server "$ACR_NAME.azurecr.io" --user-assigned $IDENTITY_NAME --min-replicas 1 --max-replicas 3 --env-vars $SETTING
az containerapp show --name $CONTAINERAPP_NAME --resource-group $RESOURCE_GROUP --query
# now open this URL in a browser and view the newly deployed web app
