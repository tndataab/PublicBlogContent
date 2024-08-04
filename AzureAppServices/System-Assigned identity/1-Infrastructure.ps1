        ##########################################################################################
# Written by Tore Nestenius
# blog at https://www.nestenius.se
# Training and consulting services at https://www.tn-data.se
##########################################################################################

. .\Settings.ps1

# Step 1: Create the resource group
Write-Host "`n`nCreating the resource group."
$resgroup = az group create `
        --name $rgname `
        --location $location `
        | ConvertFrom-Json
$resId = $resgroup.id
Write-Host "Resource group created, id: ${resId}"

# Step 2: Create Azure Container Registry
Write-Host "`n`nCreating the Azure Container Registry."
$acr = az acr create `
        --resource-group $rgname `
        --name $ACRName `
        --sku Basic `
        --admin-enabled true `
        | ConvertFrom-Json

$acrid = $acr.id
Write-Host "Azure Container Registry created, id: ${acrid}"
