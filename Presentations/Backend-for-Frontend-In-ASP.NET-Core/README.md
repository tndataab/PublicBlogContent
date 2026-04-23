# A Tokenless SPA: Secure Authentication with the BFF Pattern

Thanks for attending the talk! Here you will find everything mentioned: slides, code, and resources to take your learning further.

## Slides, Code and Links

- Slides: [Presentation.pdf](Backend-for-Frontend%20in%20ASP.NET-Core.pdf)
- Code: [Sample Demo Application](https://github.com/tndataab/PublicBlogContent/tree/main/Backend-For-Frontend)
- Blog: [Implementing the BFF Pattern in ASP.NET Core for SPAs](https://nestenius.se/net/implementing-bff-pattern-in-asp-net-core-for-spas/)
- Spec: [OAuth 2.0 for Browser-Based Applications (IETF)](https://datatracker.ietf.org/doc/html/draft-ietf-oauth-browser-based-apps)


## What the Talk Covered

This talk explored why handling OAuth/OIDC directly in the browser is risky, and how the **Backend-for-Frontend (BFF) pattern** offers a cleaner and more secure alternative.

Key topics:

- The real risks of storing tokens in the browser (XSS, token theft, refresh token abuse)
- How the BFF acts as the OAuth client so the SPA never sees a token
- Login, API call, and logout flows in a BFF architecture
- Practical defenses: HttpOnly cookies, SameSite policy, cookie prefixing, CORS lockdown, and CSRF protection
- A migration path from token-based to session-based SPA authentication
- Live ASP.NET Core implementation


## What to Do Next

**1. Read the blog series**

The talk is based on a 7-part series that goes deeper on every topic, from cookie session security and automatic token renewal to CORS hardening and the Duende BFF library:
[Implementing the BFF Pattern in ASP.NET Core for SPAs](https://nestenius.se/net/implementing-bff-pattern-in-asp-net-core-for-spas/)

**2. Download and explore the sample application**

A working ASP.NET Core demo you can clone, run, and pick apart:
[Backend-For-Frontend Demo Application](https://github.com/tndataab/PublicBlogContent/tree/main/Backend-For-Frontend)

**3. Connect with me on LinkedIn**

I post regularly about .NET, Azure, and web security. Feel free to connect or reach out with questions:
[linkedin.com/in/torenestenius](https://www.linkedin.com/in/torenestenius/)

**4. Interested in a workshop or training at your company?**

I offer in-house workshops and training on .NET, Azure, authentication, and AI. If you want to bring this (or another topic) to your team or conference, [get in touch](https://tn-data.se/contact/).


## Further Resources

- [My personal blog](https://nestenius.se/)
- [TN Datakonsult AB](https://tn-data.se/) (my consulting company)
- [Stack Overflow profile](https://stackoverflow.com/users/68490/tore-nestenius)
- [.NET Skåne](https://www.meetup.com/net-skane/) (the local .NET meetup group in southern Sweden)
