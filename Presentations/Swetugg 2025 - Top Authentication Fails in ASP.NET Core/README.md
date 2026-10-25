# Top Authentication Fails in ASP.NET Core (and How to Avoid Them)

Thanks for attending the talk! This folder contains the presentation and the demo code
from my talk at the Swetugg conference in Stockholm, February 2025.

In this talk I share the most common authentication mistakes developers make in
ASP.NET Core, especially around OpenID Connect. They include misconfigurations, protocol
misunderstandings and insecure practices. The session highlights anti-patterns, explains
how they can lead to security vulnerabilities, and offers best practices to help you build
more secure and reliable authentication systems.

## What the Talk Covered

Part 1, Data Protection:

* What the Data Protection API secures, and what happens when the key ring is missing or changes
* Several instances behind a load balancer, and persisting the key ring in shared storage
* Data Protection purposes and the application name
* Key management: who is allowed to create keys

Part 2, authentication problems:

* Authenticating over HTTP, and why it works on localhost but not on other domains
* Cookie confusion: the authentication cookie versus the session cookie
* What is inside the authentication cookie, large cookies and the cookie SessionStore
* Insecure sign-out and insecure redirects
* Trouble with the back channel to the OpenID Provider

## Slides and Code

* Slides: [Top Authentication Fails in ASP.NET Core.pdf](Top%20Authentication%20Fails%20in%20ASP.NET%20Core.pdf)
* Code, part 1: [Demo - Part1 - DAPI Base - Swetugg](Demo%20-%20Part1%20-%20DAPI%20Base%20-%20Swetugg), the starting code for the Data Protection demos
* Code, part 2: [Demo - Part2 - Base - Swetugg](Demo%20-%20Part2%20-%20Base%20-%20Swetugg), the starting code for the demos in part 2

## Further Resources

* [Persisting the ASP.NET Core Data Protection Key Ring in Azure Key Vault](https://nestenius.se/net/persisting-the-asp-net-core-data-protection-key-ring-in-azure-key-vault/)
* [Introducing the Data Protection API Key Ring Debugger](https://nestenius.se/net/introducing-the-data-protection-api-key-ring-debugger/)
* [Improving ASP.NET Core Security By Putting Your Cookies On A Diet](https://nestenius.se/net/improving-asp-net-core-security-by-putting-your-cookies-on-a-diet/)
* [Troubleshooting JwtBearer authentication issues in ASP.NET Core](https://nestenius.se/net/troubleshooting-jwtbearer-authentication-problems-in-asp-net-core/)
* [Getting Started: Authentication and Authorization in .NET](https://dometrain.com/course/getting-started-authentication-and-authorization-in-dotnet/), my video course on Dometrain. It covers the cookie handler, cookie and ticket lifetimes, and Data Protection.
* [Getting Started: OpenID Connect](https://dometrain.com/course/getting-started-openid-connect/), my video course on Dometrain. It explains how OAuth 2 and OpenID Connect work at the protocol level, including sign-out and the OpenID Provider. It is not .NET specific.
* [My personal blog](https://nestenius.se/)
* [TN Datakonsult AB](https://tn-data.se/), my consulting company
* [LinkedIn profile](https://www.linkedin.com/in/torenestenius/), feel free to connect!
* [Stack Overflow profile](https://stackoverflow.com/users/68490/tore-nestenius)
* [.NET Skåne](https://www.meetup.com/net-skane/), the local .NET meetup group in southern Sweden

Interested in a talk or training at your company? I run in-house workshops on .NET, Azure,
authentication, and OpenID Connect. [Get in touch](https://tn-data.se/contact/).
