## Data Protection API Key Ring Debugger 
This folder contains the demo code for the blog post about the DPAPI Debugger   .

**This demo project requires:**
The debugger should work for most configurations, but this example is using the Azure Key Vault as described in a earlier blog post.

To get this sample to work you need:
* You have created an instance of **Azure Key Vault**
* You have created the necessary credentials (app registration) to access it, including 
Client ID, Client Secret and the Tenant ID. Set the following environment variables: **AZURE_CLIENT_ID**, **AZURE_CLIENT_SECRET** and **AZURE_TENANT_ID** to set the **DefaultCredentials** used in the application.


## what will this demo application do?
1. At startup it will create a key ring in Azure Key Vault named **MyKeyRing**.
2. Check the **logs** produced in the console window.
3. Verify in Azure Key Vault that a new secret has been created.
4. If you restart the application, it will at startup load the key ring from the vault.
5. Optionally, you can encrypt the keys using a separate encryption key from Azure Key Vault.
6. Click on the Debugger link in the application do view the details about the current DPAPI configuration.



See my blog posts for more details at https://nestenius.se 
