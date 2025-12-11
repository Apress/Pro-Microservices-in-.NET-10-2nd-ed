
RESOURCE_GROUP = "rg-pro-microservices"
LOCATION = "eastus"
APP_SERVICE_PLAN = "sp-pro-microservices"
WEB_APP_NAME = "mywebapp$RANDOM"
APP_SKU = "F1" # Free tier
ACR_NAME = "myaccount"
IMAGE_NAME = "$ACR_NAME.azurecr.io/myimage:latest"
VNET_NAME = "myVNet"
SUBNET_NAME_APPGW = "appgwSubnet"
SUBNET_NAME_APP = "appSubnet"
APPGW_NAME = "gw-pro-microservices"
PUBLIC_IP_NAME = "myAppGwPublicIP"

# Create resource group
az group create --name $RESOURCE_GROUP --location $LOCATION

# Create ACR (optional if you already have one)
az acr create --resource-group $RESOURCE_GROUP --name $ACR_NAME --sku Basic
az acr login --name $ACR_NAME

# Create App Service Plan and Web App
az appservice plan create --name $APP_SERVICE_PLAN --resource-group $RESOURCE_GROUP --is-linux --sku $APP_SKU
az webapp create --resource-group $RESOURCE_GROUP --plan $APP_SERVICE_PLAN --name $WEB_APP_NAME --deployment-container-image-name $IMAGE_NAME

# Configure App Service to use ACR credentials
ACR_USERNAME = $(az acr credential show --name $ACR_NAME --query username --output tsv)
ACR_PASSWORD = $(az acr credential show --name $ACR_NAME --query passwords[0].value --output tsv)
az webapp config container set --name $WEB_APP_NAME --resource-group $RESOURCE_GROUP --docker-custom-image-name $IMAGE_NAME --docker-registry-server-url https://$ACR_NAME.azurecr.io --docker-registry-server-user $ACR_USERNAME --docker-registry-server-password $ACR_PASSWORD

# Set up networking for App Gateway
az network vnet create --resource-group $RESOURCE_GROUP --name $VNET_NAME --subnet-name $SUBNET_NAME_APPGW
az network public-ip create --resource-group $RESOURCE_GROUP --name $PUBLIC_IP_NAME --sku Standard

# Create and configure Application Gateway
az network application-gateway create --name $APPGW_NAME --location $LOCATION --resource-group $RESOURCE_GROUP --sku Standard_v2 --capacity 1 --vnet-name $VNET_NAME --subnet $SUBNET_NAME_APPGW --public-ip-address $PUBLIC_IP_NAME
az network application-gateway address-pool create --gateway-name $APPGW_NAME --resource-group $RESOURCE_GROUP --name "appServicePool" --servers "${WEB_APP_NAME}.azurewebsites.net"
az network application-gateway http-settings create --resource-group $RESOURCE_GROUP --gateway-name $APPGW_NAME --name "appServiceHttpSetting" --port 80 --protocol Http --pick-hostname-from-backend-address
az network application-gateway frontend-port create --resource-group $RESOURCE_GROUP --gateway-name $APPGW_NAME --name "httpPort" --port 80
az network application-gateway listener create --resource-group $RESOURCE_GROUP --gateway-name $APPGW_NAME --name "httpListener" --frontend-port "httpPort" --frontend-ip "appGatewayFrontend"
az network application-gateway rule create --resource-group $RESOURCE_GROUP --gateway-name $APPGW_NAME --name "rule1" --rule-type Basic --http-listener "httpListener" --backend-address-pool "appServicePool" --backend-http-settings "appServiceHttpSetting"
