## Backend-For-Frontend (BFF) in ASP.NET Core

This folder contains the demo code for my blog series on the Backend-For-Frontend (BFF) pattern in ASP.NET Core 9. Keeping OpenID Connect tokens in the browser puts single-page applications at risk. A BFF keeps the tokens on the server and gives the browser a secure session cookie instead.

The sample application is built up in steps: the start code, adding OpenID Connect, automatic token renewal, CORS protection, and finally replacing the custom code with the Duende BFF library. Each step is a separate project in the `BFF-Sample-application` folder.

## Blog posts

* [The BFF Pattern: What It Is and Where to Start](https://nestenius.se/bff/), the hub page with all seven parts.
* [Implementing BFF Pattern in ASP.NET Core for SPAs](https://nestenius.se/net/implementing-bff-pattern-in-asp-net-core-for-spas/), part 1: why tokens in the browser are a risk and how the BFF pattern fixes it.
* [BFF in ASP.NET Core #2 - Introducing the Backend-for-Frontend (BFF) Pattern](https://nestenius.se/net/bff-in-asp-net-core-2-the-bff-pattern-explained/), part 2: what the pattern is, where it comes from and what is inside a BFF.
* [BFF in ASP.NET Core #3 - Securing the Cookie Session](https://nestenius.se/net/bff-in-asp-net-core-3-the-bff-pattern-explained/), part 3: securing the session cookie with HttpOnly, Secure, SameSite and the `__Host-` prefix, plus security headers.
* [BFF in ASP.NET Core #4 - Implementing a BFF from scratch](https://nestenius.se/net/bff-in-asp-net-core-4-implementing-a-bff-from-scratch/), part 4: login, logout and session endpoints, OpenID Connect and a proxy for remote APIs.
* [BFF in ASP.NET Core #5 - Automatic Token Renewal](https://nestenius.se/net/bff-in-asp-net-core-5-automatic-token-renewal/), part 5: refresh tokens and the Duende.AccessTokenManagement library.
* [BFF in ASP.NET Core #6 - Securing our BFF with CORS](https://nestenius.se/net/bff-in-asp-net-core-6-securing-our-bff-with-cors/), part 6: how CORS and a required X-CSRF header protect the BFF.
* [BFF in ASP.NET Core #7 - Introducing the Duende BFF Library](https://nestenius.se/net/bff-in-asp-net-core-7-introducing-the-duende-bff-library/), part 7: replacing the custom BFF with the Duende BFF library.

## Video courses by the author

* [Getting Started: Authentication and Authorization in .NET](https://dometrain.com/course/getting-started-authentication-and-authorization-in-dotnet/), my video course on Dometrain. It covers the authentication middleware and the cookie handler.
* [Getting Started: OpenID Connect](https://dometrain.com/course/getting-started-openid-connect/), my video course on Dometrain. It explains how OAuth 2 and OpenID Connect work at the protocol level, including the OpenID Provider, claims and scopes, and sign-out.

## About the author

This code was written by [Tore Nestenius](https://nestenius.se/), a Microsoft .NET MVP and independent consultant and trainer from Göteborg, Sweden, specializing in authentication, identity and security in .NET.

* [LinkedIn](https://www.linkedin.com/in/torenestenius/)
* [Blog](https://nestenius.se/)
* [Company](https://tn-data.se/)
* [Dometrain Author](https://dometrain.com/author/tore-nestenius/)

For more details, visit my blog at https://nestenius.se
