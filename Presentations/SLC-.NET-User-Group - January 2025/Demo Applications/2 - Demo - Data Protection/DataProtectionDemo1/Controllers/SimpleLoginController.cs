using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Claims;

namespace OIDC_client.Controllers;

public class SimpleLoginController : Controller
{
    public async Task<IActionResult> Index()
    {
        // Challenge the user and sign in the user locally
        if (!User.Identity.IsAuthenticated)
        {
            await HttpContext.ChallengeAsync("cookie");
        }

        return View();
    }

    [HttpGet]
    public IActionResult Login(string ReturnUrl)
    {
        return View(new LoginModel() { ReturnUrl = ReturnUrl });
    }


    /// <summary>
    /// Simple login, with fixed user details.
    /// </summary>
    /// <param name="loginCredentials"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel loginCredentials)
    {
        //1. Validate username + password

        //2. Load the claims for this user from the DB
        var claims = new List<Claim>()
          {
              new("sub","1234"),
              new("name","Bob"),
              new("email","bob@tn-data.se"),
              new("role","developer")
          };

        var identity = new ClaimsIdentity(claims: claims,
                                          authenticationType: "pwd",
                                          nameType: "name",
                                          roleType: "role");

        var principal = new ClaimsPrincipal(identity);

        var userAgent = HttpContext.Request.Headers.UserAgent.ToString();
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

        var prop = new AuthenticationProperties()
        {
            Items =
              {
                  { "UserAgent", userAgent },
                  { "IpAddress", ipAddress },
                  { "ComputerName", "MyComputer" },
                  { "ApiKey", "Summer2024!!" }
              }
        };

        await HttpContext.SignInAsync(scheme: "cookie",
                                      principal: principal,
                                      properties: prop);

        return LocalRedirect(loginCredentials.ReturnUrl);

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task Logout(string returnUrl)
    {
        var prop = new AuthenticationProperties()
        {
            RedirectUri = "/"
        };

        //Sign out from the specific scheme
        await HttpContext.SignOutAsync("cookie", prop);
    }
}

