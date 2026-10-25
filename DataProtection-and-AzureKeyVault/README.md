## Securing ASP.NET Core Data Protection Keys with Azure Key Vault 
This folder contains the demo code for the blog post about storing the DPAPI key ring in Azure Key Vault.

**This demo project requires:**
* You have created an instance of **Azure Key Vault**
* You have created the necessary credentials (app registration) to access it, including 
Client ID, Client Secret and the Tenant ID. Set the following environment variables: **AZURE_CLIENT_ID**, **AZURE_CLIENT_SECRET** and **AZURE_TENANT_ID** to set the **DefaultCredentials** used in the application.


## what will this demo application do?
1. At startup it will create a key ring in Azure Key Vault named **MyKeyRing**.
2. Check the **logs** produced in the console window.
3. Verify in Azure Key Vault that a new secret has been created.
4. If you restart the application, it will at startup load the key ring from the vault.
5. Optionally, you can encrypt the keys using a separate encryption key from Azure Key Vault.


## Blog posts

* [Persisting the ASP.NET Core Data Protection Key Ring in Azure Key Vault](https://nestenius.se/net/persisting-the-asp-net-core-data-protection-key-ring-in-azure-key-vault/), the post for this demo
* [Introducing the Data Protection API Key Ring Debugger](https://nestenius.se/net/introducing-the-data-protection-api-key-ring-debugger/), a tool to inspect the key ring and the Data Protection setup

## Learn more

* [Getting Started: Authentication and Authorization in .NET](https://dometrain.com/course/getting-started-authentication-and-authorization-in-dotnet/), my video course on Dometrain. It covers the authentication middleware and the cookie handler, and has a lesson on Data Protection.

See my blog for more at https://nestenius.se
