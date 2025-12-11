
RESOURCE_GROUP = "pro-microservices"
LOCATION = "canadacentral"
PRICE_TIER = "F1"
ACR_NAME = "my-registry"  # Just the name, not full URL
ACR_LOGIN_SERVER = "$ACR_NAME.azurecr.io"
APP_SERVICE_PLAN = "app-plan"
WEBAPP_NAME = "my-webapp"
IMAGE_NAME = "myimage:v0.1"
SETTING = "ASPNETCORE_ENVIRONMENT = Production"

# if not already, `az login` and `az account set --subscription "..."`
az group create --name "$RESOURCE_GROUP" --location "$LOCATION"
az appservice plan create --name "$APP_SERVICE_PLAN" --resource-group "$RESOURCE_GROUP" --is-linux --sku "$PRICE_TIER"
az appservice plan show --name "$APP_SERVICE_PLAN" --resource-group "$RESOURCE_GROUP" -o table
# create an empty webapp to avoid chicken-and-egg problem between ACR and webapp
az webapp create --resource-group "$RESOURCE_GROUP" --plan "$APP_SERVICE_PLAN" --name "$WEBAPP_NAME" --runtime "DOTNETCORE:8.0"
az webapp identity assign --name "$WEBAPP_NAME" --resource-group "$RESOURCE_GROUP"
ACR_ID = $(az acr show --name "$ACR_NAME" --query id --output tsv)
WEBAPP_IDENTITY = $(az webapp show --name "$WEBAPP_NAME" --resource-group "$RESOURCE_GROUP" --query identity.principalId --output tsv)
az role assignment create --assignee "$WEBAPP_IDENTITY" --role AcrPull --scope "$ACR_ID"
az webapp config container set --name "$WEBAPP_NAME" --resource-group "$RESOURCE_GROUP" --docker-custom-image-name "$ACR_LOGIN_SERVER/$IMAGE_NAME" --docker-registry-server-url "https://$ACR_LOGIN_SERVER"
az webapp config appsettings set --name "$WEBAPP_NAME" --resource-group "$RESOURCE_GROUP" --settings "$SETTING"
az webapp config appsettings list --name "$WEBAPP_NAME" --resource-group "$RESOURCE_GROUP"
az webapp show --name "$WEBAPP_NAME" --resource-group "$RESOURCE_GROUP" --query defaultHostName --output tsv
# now open this URL in a browser and view the newly deployed web app
