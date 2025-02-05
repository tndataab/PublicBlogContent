using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace OIDC_client.Controllers;

public class UserController : Controller
{
    [HttpGet]
    public async Task Login()
    {
        var userAgent = HttpContext.Request.Headers.UserAgent.ToString();
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

        var prop = new AuthenticationProperties()
        {
            RedirectUri = "/",
            Items =
                {
                    { "UserAgent", userAgent },
                    { "IpAddress", ipAddress }
                }
        };

        await HttpContext.ChallengeAsync("oidc", prop);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout(string returnUrl)
    {
        var user = HttpContext.User;
        var authenticationType = user.Identity?.AuthenticationType;

        if (authenticationType == "pwd")
            return SignOut(new AuthenticationProperties { RedirectUri = "/" }, "cookie");
        else
        {
            return SignOut(new AuthenticationProperties { RedirectUri = "/" }, "cookie", "oidc");
        }
    }

    public IActionResult LoggedOut()
    {
        return View();
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    [Authorize]
    public IActionResult Info()
    {
        string idToken = HttpContext.GetTokenAsync("id_token").Result ?? "";
        string accessToken = HttpContext.GetTokenAsync("access_token").Result ?? "";
        string refreshToken = HttpContext.GetTokenAsync("refresh_token").Result ?? "";

        //To prevent XSS, make sure the token only contains valid base64 characters or ".-"
        //https://en.wikipedia.org/wiki/Base64#Variants_summary_table
        var regex = new Regex(@"^[\w\+\/\=\.-]+$");  //Matches a-z, A-Z, 0-9, including the _ (underscore) character.

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

