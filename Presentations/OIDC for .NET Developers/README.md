# OpenID Connect for .NET Developers

Thanks for attending the talk! This folder contains the presentation and the sample demo
code used during the session.

Adding OpenID Connect to ASP.NET Core often starts with a call to `AddOpenIdConnect()`. But
what actually happens under the hood?

In this session we build a practical mental model of OpenID Connect and explore the three
key roles of the OpenID Connect handler: starting the authentication flow, handling the
callback and obtaining tokens, and signing the user out. Along the way we see how the
application learns who the user is and obtains access to protected APIs. The talk gives you
a solid starting point for understanding and using OpenID Connect in your ASP.NET Core
applications.

## Slides and Code

* Slides: [OIDC for .NET Developers.pdf](OIDC%20for%20.NET%20Developers.pdf)
* Code: [Sample Demo App - Solution](Sample%20Demo%20App%20-%20Solution), the completed demo
  application with OpenID Connect wired up

## Further Resources

* [Demystifying OpenID Connect's State & Nonce Parameters in ASP.NET Core](https://nestenius.se/net/demystifying-openid-connects-state-and-nonce-parameters-in-asp-net-core/)
* [Debugging OpenID Connect Claim Problems in ASP.NET Core](https://nestenius.se/net/missing-openid-connect-claims-in-asp-net-core/)
* [OpenID Connect and OAuth: What They Are and Where to Start](https://nestenius.se/openid-connect/)
* [Getting Started: OpenID Connect](https://dometrain.com/course/getting-started-openid-connect/?ref=tore-nestenius), my video course on Dometrain. It explains how OAuth 2 and OpenID Connect work at the protocol level: tokens, signing and the flows. It is not .NET specific.
* [My personal blog](https://nestenius.se/)
* [TN-Data](https://tn-data.se/), my consulting company
* [Dometrain author page](https://dometrain.com/author/tore-nestenius/?ref=tore-nestenius), my video courses
* [LinkedIn profile](https://www.linkedin.com/in/torenestenius/), feel free to connect!
* [.NET Skåne](https://www.meetup.com/net-skane/), the local .NET meetup group in southern Sweden

Interested in a talk or training at your company? I run in-house workshops on .NET, Azure,
authentication, and OpenID Connect. [Get in touch](https://tn-data.se/contact/).
