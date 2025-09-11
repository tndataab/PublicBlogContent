using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace OIDC_client.Controllers;

public class UserController : Controller
{
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
    new("sub","1234" + new string('x',10000)),
    new("name","Bob"),
    new("email","bob@tn-data.se"),
    new("role","developer")
};


        var identity = new ClaimsIdentity(claims: claims,
                                          authenticationType: "pwd",
                                          nameType: "name",
                                          roleType: "role");

        var principal = new ClaimsPrincipal(identity);

        var prop = new AuthenticationProperties()
        {
            RedirectUri = "/",
            Items =
    {
        { "IpAddress", "192.168.0.3" },
        { "ComputerName", "MyComputer" },
        { "ApiKey", "Summer2025!!" }
    }
        };

        await HttpContext.SignInAsync(scheme: "cookie",
                                      principal: principal,
                                      properties: prop);
        return LocalRedirect("/");

    }


    [HttpPost]
    public async Task Logout()
    {
        await HttpContext.SignOutAsync("cookie");
    }


    public IActionResult LoggedOut()
    {
        return View();
    }


    public IActionResult AccessDenied()
    {
        return View();
    }


    public IActionResult Info()
    {
        string idToken = "";
        string accessToken = "";
        string refreshToken = "";

        if (User.Identity.IsAuthenticated)
        {
            idToken = HttpContext.GetTokenAsync("id_token").Result ?? "";
            accessToken = HttpContext.GetTokenAsync("access_token").Result ?? "";
            refreshToken = HttpContext.GetTokenAsync("refresh_token").Result ?? "";
        }

        //To prevent XSS, make sure the token only contains valid base64 characters or ".-"
        //https://en.wikipedia.org/wiki/Base64#Variants_summary_table
        Regex regex = new Regex(@"^[\w\+\/\=\.-]+$");  //Matches a-z, A-Z, 0-9, including the _ (underscore) character.

        if (regex.Match(idToken).Success)
        {
            ViewData["idToken"] = idToken;
        }
        if (regex.Match(accessToken).Success)
        {
            ViewData["accessToken"] = accessToken;
        }
        if (regex.Match(refreshToken).Success)
        {
            ViewData["refreshToken"] = refreshToken;
        }

        return View();
    }
}

