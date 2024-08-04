## Deploy a container to Azure App Services using Azure CLI and managed identity
This folder contains the code for my blog post about deploying container images to Azure App Services.

### \User-assigned identitites
This folder contains the script for deploying a container using user-assigned managed identitites.


* **Settings.ps1** contains the various settings used by the other scripts.
* **1-Infrastructure.ps1** will create the following
  * Resource group
  * Managed identity
  * Azure Container Registry
  * Assign the AcrPull role to the managed identity to allow container Pull from the ACR
* **2-BuildAndPushDockerImage.ps1** will do the following:
  * Login to the Azure Container Registry
  * Tag the local Docker image with the registry server name of your ACR
  * Push the local image to Azure Container Registry  
   
  Here you must modify script so it uses to your local container image.  

* **3-CreateAppService-UserAssigned.ps1** will do the following:
  * Get the details about the existing managed identity
  * Create the Linux App Service Plan
  * Create container App Service
  * Set the user-assigned identity in the App Service for accessing the ACR  
  * Verify the ACR access settings  
  * Enable Application and container logging (Filesystem)
  
*   **DeleteAllResources.ps1** will delete the resource group and all of its content.

Visit https://nestenius.se to read the blog post.

