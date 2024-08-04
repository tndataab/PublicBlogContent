##########################################################################################
# Written by Tore Nestenius
# blog at https://www.nestenius.se
# Training and consulting services at https://www.tn-data.se
##########################################################################################

# Create the App Service

#Run the settings script
. .\Settings.ps1


# Step 1: Query for the Azure Container Registry ID
Write-Host "`nQuerying for the container registry ID"
$acr = az acr show `
    --name $acrName `
    --resource-group $rgname `
    --output json | ConvertFrom-Json
$acrId = $acr.id
Write-Host "ACR found with ID: ${acrId}"

# Step 2: Create the Linux App Service Plan
Write-Host "`nCreating the Linux App Service Plan"
$servicePlan = az appservice plan create `
    --name $AppServicePlan_linux `
    --resource-group $rgname `
    --is-linux `
    --sku $AppServicePlanSKU_Linux `
    --output json | ConvertFrom-Json
$servicePlanId = $servicePlan.id
Write-Host "App Service Plan created with id: ${servicePlanId}"

# Step 3: Create container App Service
$imagePath = "${acrname}.azurecr.io/${imagename}:latest"
Write-Host "`n`nCreating the container App Service."
Write-Host "With the following image ${imagePath}"
$AppService = az webapp create `
    --name $AppServiceName_container_linux `
    --acr-use-identity `
    --plan $AppServicePlan_linux `
    --resource-group $rgname `
    --container-image-name $imagePath `
    --assign-identity [system] `
    --role "AcrPull" `
    --scope $acrId `
    --output json | ConvertFrom-Json
$hostName = $AppService.defaultHostName
$appServiceID = $AppService.id
Write-Host "App Service created, id: ${appServiceID}"


# Step 4: Verify the ACR access settings  
$settings = az webapp config show `
    --resource-group $rgname `
    --name $AppServiceName_container_linux `
    --output json | ConvertFrom-Json
Write-Host "`nThese two settings must be set for successful ACR pull:"
Write-Host "acrUseManagedIdentityCreds='$($settings.acrUseManagedIdentityCreds)'"
Write-Host "acrUserManagedIdentityID='$($settings.acrUserManagedIdentityId)'"


# Step 5: Enable Application and container logging (Filesystem)
Write-Host "`nEnabling application logging."
$tmp = az webapp log config `
    --name $AppServiceName_container_linux `
    --resource-group $rgname `
    --application-logging filesystem `
    --docker-container-logging filesystem `
    --level verbose `
    --output json | ConvertFrom-Json

# Final Output
Write-Host "App service URL: https://${hostName}" -ForegroundColor Green

#Optionally enable this to run log stream for debugging purposes
az webapp log tail `
        --name $AppServiceName_container_linux `
        --resource-group $rgname


        