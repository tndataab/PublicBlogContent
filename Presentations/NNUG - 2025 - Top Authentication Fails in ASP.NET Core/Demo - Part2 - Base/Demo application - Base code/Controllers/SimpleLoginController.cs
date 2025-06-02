using Demo_application___Base_code.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace Demo_application___Base_code.Controllers;

/// <summary>
/// Simple login using the Cookie Handler
/// 
/// Written by Tore Nestenius
/// https://nestenius.se
/// </summary>
public class SimpleLoginController : Controller
{
    public async Task<IActionResult> Index()
    {
        // Challenge the user and sign in the user locally, without OIDC
        if (User.Identity.IsAuthenticated == false)
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

        return Redirect(loginCredentials.ReturnUrl);

    }
}

