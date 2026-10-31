## Data Protection API Key Ring Debugger 
This folder contains the demo code for the blog post about the DPAPI Debugger   .

**This demo project requires:**
The debugger should work for most configurations, but this example is using the Azure Key Vault as described in an earlier blog post.

To get this sample to work you need:
* You have created an instance of **Azure Key Vault**
* You have created the necessary credentials (app registration) to access it, including 
Client ID, Client Secret and the Tenant ID. Set the following environment variables: **AZURE_CLIENT_ID**, **AZURE_CLIENT_SECRET** and **AZURE_TENANT_ID** to set the **DefaultCredentials** used in the application.


## What will this demo application do?
1. At startup it will create a key ring in Azure Key Vault named **MyKeyRing**.
2. Check the **logs** produced in the console window.
3. Verify in Azure Key Vault that a new secret has been created.
4. If you restart the application, it will at startup load the key ring from the vault.
5. Optionally, you can encrypt the keys using a separate encryption key from Azure Key Vault.
6. Click on the Debugger link in the application to view the details about the current DPAPI configuration.



## Blog posts

* [Introducing the Data Protection API Key Ring Debugger](https://nestenius.se/net/introducing-the-data-protection-api-key-ring-debugger/), the post for this demo.
* [Persisting the ASP.NET Core Data Protection Key Ring in Azure Key Vault](https://nestenius.se/net/persisting-the-asp-net-core-data-protection-key-ring-in-azure-key-vault/), the earlier post this demo builds on.

## Video courses by the author

* [Getting Started: Authentication and Authorization in .NET](https://dometrain.com/course/getting-started-authentication-and-authorization-in-dotnet/), my video course on Dometrain. It covers the authentication middleware and the cookie handler, and gives a short introduction to the Data Protection API.

## About the author

This code was written by [Tore Nestenius](https://nestenius.se/), a Microsoft .NET MVP and independent consultant and trainer from Göteborg, Sweden, specializing in authentication, identity and security in .NET.

* [LinkedIn](https://www.linkedin.com/in/torenestenius/)
* [Blog](https://nestenius.se/)
* [Company](https://tn-data.se/)
* [Dometrain Author](https://dometrain.com/author/tore-nestenius/)

See my blog for more at https://nestenius.se
