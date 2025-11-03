# Øredev - How Do You Get an Access Token in Azure? 
This folder contains the presentation from my talk at the <a href="https://oredev.org" target="_blank">Øredev 2025</a> conference in Malmö, Sweden.

## The .NET Developer’s Guide to Token Credentials
If you have developed .NET apps that connect to Azure, you probably used DefaultAzureCredential.

It seems straightforward: just create one, and you’re done. But behind that single class is a surprising amount of complexity. 
It manages nearly 20 different TokenCredential types, each trying to get an access token in its own way.
 
This session will explain how authentication works in the Azure SDK for .NET. You will learn what the TokenCredential objects do, 
why the DefaultAzureCredential is so convenient, and when it might fail. We’ll also show you how to create a custom flow with ChainedTokenCredential for more control.
 
By the end of this talk, you’ll have a solid understanding of Azure authentication in .NET. You will know how to troubleshoot your credential 
chain and how to peek behind the scenes with DefaultAzureCredential. 

Join us to clear up the confusion once and for all!



## Resources
* <a href="https://github.com/tndata/CloudDebugger" target="_blank">Cloud Debugger</a>
* <a href="https://nestenius.se/azure/default-azure-credentials-under-the-hood/" target="_blank">DefaultAzureCredentials Under the Hood</a> blog post.


### Further resources
* <a href="https://nestenius.se/" target="_blank">My personal blog</a>
* <a href="https://tn-data.se/" target="_blank">TN Datakonsult</a>, my personal training and consulting company.
* <a href="https://www.linkedin.com/in/torenestenius/" target="_blank">Linkedin profile</a>, fee free to connect!
* <a href="https://stackoverflow.com/users/68490/tore-nestenius" target="_blank">Stack Overflow</a> profile

 
 
