## Deploy a container to Azure App Services using Azure CLI and managed identity

This folder contains the PowerShell scripts for deploying a container image from a private Azure Container Registry (ACR) to Azure App Service with Azure CLI. The App Service pulls the image using a managed identity with the AcrPull role, instead of registry credentials.

There are two versions, depending on the type of managed identity: **User-assigned identity** and **System-assigned identity**.

## Blog posts

* [Deploy containers to Azure App Services using user-assigned managed identity](https://nestenius.se/azure/deploy-a-container-to-azure-app-services-using-azure-cli-and-user-assigned-managed-identity/), how to deploy a container from a private ACR with Azure CLI, using a user-assigned managed identity to pull the image.
* [Deploy Container to Azure App Services with System-Assigned Identity](https://nestenius.se/azure/deploy-a-container-to-azure-app-services-using-a-system-assigned-identity/), the same deployment using a system-assigned managed identity.

### User-assigned identity
This folder contains the script for deploying a container using a user-assigned managed identity.


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
  
* **4-CreateAppService-UserAssigned-version2.ps1** contains an improved version with the following change: 
  * It first deploys the built-in runtime image, and then deploys the container. 

  
*   **DeleteAllResources.ps1** will delete the resource group and all of its content.


### System-assigned identity
This folder contains the script for deploying a container using a system-assigned managed identity.

* **Settings.ps1** contains the various settings used by the other scripts.
* **1-Infrastructure.ps1** will create the following
  * Resource group
  * Azure Container Registry
* **2-BuildAndPushDockerImage.ps1** will do the following:
  * Login to the Azure Container Registry
  * Tag the local Docker image with the registry server name of your ACR
  * Push the local image to Azure Container Registry  
   
  Here you must modify script so it uses to your local container image.  

* **3-CreateAppService-UserAssigned.ps1** will do the following:
  * Get the details about the existing container registry.
  * Create the Linux App Service Plan
  * Create container App Service
  * Enable Application and container logging (Filesystem)
  

## Related Azure blog posts by the author

* [DefaultAzureCredential Under the Hood](https://nestenius.se/azure/default-azure-credentials-under-the-hood/), how DefaultAzureCredential works in .NET, the order it tries credentials and which one it used.
* [Introducing the Cloud Debugger for Azure](https://nestenius.se/azure/introducing-the-cloud-debugger-for-azure/), an open-source tool I built to explore and troubleshoot Azure, with over 30 tools for App Service, storage, messaging and identity.
* [Running Docker in an Azure Windows Virtual Machine – Not so fast!](https://nestenius.se/azure/running-docker-in-an-azure-windows-virtual-machine-not-so-fast/), why Docker won't start in a new Azure Windows VM: Trusted Launch, the default security type, disables virtualization.
* [User Delegation SAS Tokens In Azure Explained](https://nestenius.se/azure/user-delegation-sas-tokens-in-azure-explained/), what a user delegation SAS is in Azure Storage, why it beats a SAS signed with the account key, and how to create one in .NET with a managed identity.

## About the author

This code was written by [Tore Nestenius](https://nestenius.se/), a Microsoft .NET MVP and independent consultant and trainer from Göteborg, Sweden, specializing in authentication, identity and security in .NET.

* [LinkedIn](https://www.linkedin.com/in/torenestenius/)
* [Blog](https://nestenius.se/)
* [Company](https://tn-data.se/)
* [Dometrain Author](https://dometrain.com/author/tore-nestenius/?ref=tore-nestenius)


