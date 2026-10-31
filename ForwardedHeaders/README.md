# ASP.NET Core Forwarded Headers Middleware: demo code

This folder contains the source code for my blog series on the Forwarded Headers Middleware in ASP.NET Core. Behind a reverse proxy or load balancer, your app sees the proxy's address, scheme and host instead of the client's. The middleware reads the X-Forwarded headers to restore the original client IP address, scheme and host.

The two projects each return the details of the incoming request as JSON, so you can see what the middleware changes:

* **ForwardedHeaders - Part 1** configures the middleware in code (headers to process, known proxies and networks).
* **ForwardedHeaders - Part 2** reads the settings from `appsettings.json`, and covers the `ASPNETCORE_FORWARDEDHEADERS_ENABLED` environment variable and the `FORWARDEDHEADERS_ENABLED` configuration setting.

Each project has a `TestRequest.http` file for sending test requests.

## Blog posts

* [Exploring the Forwarded Headers Middleware in ASP.NET Core](https://nestenius.se/net/exploring-the-forwarded-headers-middleware-in-asp-net-core/), what the middleware does behind a proxy and how the X-Forwarded headers restore the client IP, scheme and host.
* [Configuring ASP.NET Core Forwarded Headers Middleware](https://nestenius.se/net/configuring-asp-net-core-forwarded-headers-middleware/), which headers to process, which proxies to trust, and how `ASPNETCORE_FORWARDEDHEADERS_ENABLED` and `AllowedHosts` fit in.

## About the author
This code was written by [Tore Nestenius](https://nestenius.se/), a Microsoft .NET MVP and independent consultant and trainer from Göteborg, Sweden, specializing in authentication, identity and security in .NET.

* [LinkedIn](https://www.linkedin.com/in/torenestenius/)
* [Blog](https://nestenius.se/)
* [Company](https://tn-data.se/)
* [Dometrain Author](https://dometrain.com/author/tore-nestenius/)

## Related blog posts by the author

* [The BFF Pattern: What It Is and Where to Start](https://nestenius.se/bff/), a seven-part series on the Backend-for-Frontend (BFF) pattern. A BFF is a form of proxy that sits between the browser and your APIs.
* [IdentityServer in Docker Containers](https://nestenius.se/net/identityserver-in-docker-containers-part-1/), a series where the networking and HTTPS problems that appear behind a proxy or between containers are solved step by step.

## Video courses by the author

* [Getting Started: Authentication and Authorization in .NET](https://dometrain.com/course/getting-started-authentication-and-authorization-in-dotnet/), my video course on Dometrain. It covers the authentication middleware and the cookie handler.
* [Getting Started: OpenID Connect](https://dometrain.com/course/getting-started-openid-connect/), my video course on Dometrain. It explains how OAuth 2 and OpenID Connect work at the protocol level, including the OpenID Provider, claims and scopes, and sign-out.
