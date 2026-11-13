# Exploring Authentication in ASP.NET Core

Thanks for attending the talk! This folder contains the presentation and the demo code
from my talk at NDC Copenhagen, September 2025.

Authentication in ASP.NET Core has many concepts and moving parts. In this talk we look at
how it works and go through the key ideas: schemes, handlers, the ClaimsPrincipal, claims,
challenges, session cookies and more. Along the way we build up a sample application with
live coding.

## What the Talk Covered

* The ClaimsPrincipal, claims and where the user comes from in the request pipeline
* The authentication middleware and its operations: Authenticate, Challenge, SignIn, SignOut and Forbid
* The cookie handler, signing a user in and out, and return URLs after logout
* Schemes and using more than one handler
* Large cookies

## Slides and Code

* Slides: [Exploring Authentication in ASP.NET Core.pdf](Exploring%20Authentication%20in%20ASP.NET%20Core.pdf)
* Code: [Auth - demo application](Auth%20-%20demo%20application), the demo application used in the talk

## Further Resources

* [Improving ASP.NET Core Security By Putting Your Cookies On A Diet](https://nestenius.se/net/improving-asp-net-core-security-by-putting-your-cookies-on-a-diet/)
* [Exploring what is inside the ASP.NET Core cookies](https://nestenius.se/net/exploring-what-is-inside-the-asp-net-core-cookies/)
* [Debugging cookie problems in ASP.NET Core](https://nestenius.se/net/debugging-cookie-problems/)
* [Getting Started: Authentication and Authorization in .NET](https://dometrain.com/course/getting-started-authentication-and-authorization-in-dotnet/?ref=tore-nestenius), my video course on Dometrain. It explains what happens inside ASP.NET Core: the ClaimsPrincipal, the authentication middleware and the cookie handler.
* [My personal blog](https://nestenius.se/)
* [TN-Data](https://tn-data.se/), my consulting company
* [Dometrain author page](https://dometrain.com/author/tore-nestenius/?ref=tore-nestenius), my video courses
* [LinkedIn profile](https://www.linkedin.com/in/torenestenius/), feel free to connect!
* [.NET Skåne](https://www.meetup.com/net-skane/), the local .NET meetup group in southern Sweden

Interested in a talk or training at your company? I run in-house workshops on .NET, Azure,
authentication, and OpenID Connect. [Get in touch](https://tn-data.se/contact/).
