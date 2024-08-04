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

# Step 2: Create a managed identity
Write-Host "`nCreating a managed identity."
$identity = az identity create `
        --name $identityName `
        --resource-group $rgname `
        --output json | ConvertFrom-Json
$identityId = $identity.id
$principalId = $identity.principalId
Write-Host "User-assigned managed identity created"
Write-Host "name: ${identityName}"
Write-Host "id: ${identityId}"
Write-Host "PrincipalId: ${principalId}"

Write-Host "`nWaiting 30s to ensure the identity is fully registered and propagated in Azure AD..."
# The AcrPull assignment might otherwise fail.
Start-Sleep -Seconds 30

# Step 3: Create Azure Container Registry
Write-Host "`n`nCreating the Azure Container Registry."
$acr = az acr create `
        --resource-group $rgname `
        --name $ACRName `
        --sku Basic `
        --admin-enabled true `
        --output json | ConvertFrom-Json

$acrid = $acr.id
Write-Host "Azure Container Registry created, id: ${acrid}"

# Step 4: Assign the AcrPull role to the managed identity on the ACR
Write-Host "`n`nAssigning AcrPull role to the managed identity."
$role = az role assignment create `
        --assignee $principalId `
        --role "AcrPull" `
        --scope $acrid `
        --output json `
        --output json | ConvertFrom-Json
