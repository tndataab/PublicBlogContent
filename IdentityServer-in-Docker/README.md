# IdentityServer in Docker – Step-by-Step Guide Source Code

## Background
This folder contains the source code for the IdentityServer in Docker blog series. Running Duende IdentityServer and a client in separate Docker containers breaks login in ways that are hard to spot: localhost no longer reaches the other container, the issuer does not match, cookies need HTTPS and logout fails. It includes both the initial baseline code and the final containerized solution, allowing you to follow along and understand the process of containerizing IdentityServer step-by-step.

## Start Project
The Start Project folder contains the baseline source code, which you can run locally without Docker. You should be able to authenticate successfully using the credentials bob/bob. This serves as the initial starting point that will be gradually containerized through various attempts in the blog series.

## Final Code
The Final Code folder contains the completed solution, showcasing a fully containerized setup with IdentityServer running in Docker. This includes all configurations and optimizations made throughout the guide.

## Important
This blog post focuses on getting the application to work locally in a containerized environment using Docker Compose. Making this setup production-ready involves additional considerations, which are beyond the scope of this post. We aim to successfully run and test the setup in a local development environment and understand the critical decisions made.



## The blog series

* [IdentityServer in Docker Containers - Part 1](https://nestenius.se/net/identityserver-in-docker-containers-part-1/)
* [IdentityServer in Docker Containers: Networking (Part 2)](https://nestenius.se/net/identityserver-in-docker-containers-part-2/)
* [IdentityServer in Docker Containers: HTTPS and SameSite (Part 3)](https://nestenius.se/net/identityserver-in-docker-containers-part-3/)
* [IdentityServer In Docker Containers – Handle Logout (Part 4)](https://nestenius.se/net/identityserver-in-docker-containers-part-4/)
* [IdentityServer: What It Is and Where to Start](https://nestenius.se/identityserver/)

## About the author
This code was written by [Tore Nestenius](https://nestenius.se/), a Microsoft .NET MVP and independent consultant and trainer from Göteborg, Sweden, specializing in authentication, identity and security in .NET.

* [LinkedIn](https://www.linkedin.com/in/torenestenius/)
* [Blog](https://nestenius.se/)
* [Company](https://tn-data.se/)
* [Dometrain Author](https://dometrain.com/author/tore-nestenius/?ref=tore-nestenius)

## Related blog posts by the author
* [Duende IdentityServer 7: A Complete Setup Guide for ASP.NET Core](https://nestenius.se/net/duende-identityserver-7-a-complete-setup-guide-for-asp-net-core/), setting up Duende IdentityServer 7 from scratch. The demo code is in the [DuendeIdentityServer7](../DuendeIdentityServer7) folder.
* [IdentityServer – IdentityResource vs. ApiResource vs. ApiScope](https://nestenius.se/net/identityserver-identityresource-vs-apiresource-vs-apiscope/)
* [Demystifying OpenID Connect’s State and Nonce Parameters in ASP.NET Core](https://nestenius.se/net/demystifying-openid-connects-state-and-nonce-parameters-in-asp-net-core/)
* [Improving ASP.NET Core Security By Putting Your Cookies On A Diet](https://nestenius.se/net/improving-asp-net-core-security-by-putting-your-cookies-on-a-diet/)

## Video courses by the author

* [Getting Started: Authentication and Authorization in .NET](https://dometrain.com/course/getting-started-authentication-and-authorization-in-dotnet/?ref=tore-nestenius), my video course on Dometrain. It covers the authentication middleware and the cookie handler.
* [Getting Started: OpenID Connect](https://dometrain.com/course/getting-started-openid-connect/?ref=tore-nestenius), my video course on Dometrain. It explains how OAuth 2 and OpenID Connect work at the protocol level, including the OpenID Provider, claims and scopes, and sign-out.

