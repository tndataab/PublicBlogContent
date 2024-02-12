## Securing ASP.NET Core Data Protection Keys with Azure Key Vault 
This folder contains the demo code for the blog post about storing the DPAPI key ring in Azure Key Vault.

**This demo project requires:**
* You have created an instance of **Azure Key Vault**
* You have created the necessary credentials (app registration) to access it, including 
Client ID, Client Secret and the Tenant ID. One option for the DefaultCredentials to work is to set the following environment variables: **AZURE_CLIENT_ID**, **AZURE_CLIENT_SECRET** and **AZURE_TENANT_ID**.


## what will this demo application do?
1. At startup it will create a key ring in Azure Key Vault named **MyKeyRing**.
2. Check the **logs** produced in the console window.
3. Verify in Azure Key Vault that a new secret has been created.
4. If you restart the application, it will at startup load the key ring from the vault.


See my blog posts for more details at https://nestenius.se
