##########################################################################################
# Written by Tore Nestenius
# blog at https://www.nestenius.se
# Training and consulting services at https://www.tn-data.se
##########################################################################################

# resource group name
$rgname = 'MyTestResourceGroup'

# location
$location = 'swedencentral'

# The name of the App Service Plans
$AppServicePlan_linux = 'asp-MyApp-Linux-dev'

# The SKU of the App Service Plans
$AppServicePlanSKU_Linux = 'S1'     # Standard plan 

# The name of the App Services
$AppServiceName_container_linux = 'MyApp-Linux-Container-dev'

# Azure container registry name
$ACRName = 'tncontaineregistry'

# AzureRover container image name
$imagename = 'mycontainerimage'

