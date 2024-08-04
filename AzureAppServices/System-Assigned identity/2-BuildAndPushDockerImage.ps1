##########################################################################################
# Written by Tore Nestenius
# blog at https://www.nestenius.se
# Training and consulting services at https://www.tn-data.se
##########################################################################################

. .\Settings.ps1

# Step 1: Log in to the Azure Container Registry
Write-Host "`n`nLogging into Azure Container Registry '${ACRName}'."
Write-Host "If this step hangs, ensure Docker is running locally and it is not suspended."
az acr login --name $ACRName

# Step 2 Tag the local Docker image with the registry server name of your ACR
$taggedImage = "${ACRName}.azurecr.io/${imagename}:latest"
$localImageName = "azure-rover" 
Write-Host "`n`nTagging the Docker image '${localImageName}' with '${taggedImage}'."
docker tag $localImageName $taggedImage

# Step 3: Push the local image to Azure Container Registry
Write-Host "`n`nPushing the Docker image '${taggedImage}' to ACR."
docker push $taggedImage
