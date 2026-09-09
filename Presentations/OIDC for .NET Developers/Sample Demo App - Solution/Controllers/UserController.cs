using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Starter_Project.Controllers;

public class UserController : Controller
{
    /// <summary>
    /// Starts the login. Empty for now - the presentation fills this in with a
    /// ChallengeAsync against the "oidc" scheme.
    /// </summary>
    [HttpPost]
    public async Task Login()
    {
        var prop = new AuthenticationProperties
        {
            RedirectUri = "/"
        };

        // Ask the user to sign in
        await HttpContext.ChallengeAsync("oidc", prop);
    }


    /// <summary>
    /// Signs the user out. Empty for now - the presentation fills this in with a
    /// SignOutAsync against the cookie scheme and then the "oidc" scheme.
    /// </summary>
    [HttpPost]
    public Task Logout()
    {
        return Task.CompletedTask;
    }

    public async Task<IActionResult> Info()
    {
        // Until authentication is wired up there is no scheme to read tokens from, and
        // calling GetTokenAsync would throw. Once AddCookie/AddOpenIdConnect are in place
        // and a user has signed in, this block runs for real and fills in the tokens.
        if (User.Identity?.IsAuthenticated == true)
        {
            string idToken = await HttpContext.GetTokenAsync("id_token") ?? "";
            string accessToken = await HttpContext.GetTokenAsync("access_token") ?? "";
            string refreshToken = await HttpContext.GetTokenAsync("refresh_token") ?? "";

            //To prevent XSS, make sure the token only contains valid base64 characters or ".-"
            //https://en.wikipedia.org/wiki/Base64#Variants_summary_table
            var regex = new Regex(@"^[\w\+\/\=\.-]+$");  //Matches a-z, A-Z, 0-9, including the _ (underscore) character.

            if (regex.IsMatch(idToken))
            {
                ViewData["idToken"] = idToken;
            }
            if (regex.IsMatch(accessToken))
            {
                ViewData["accessToken"] = accessToken;
            }
            if (regex.IsMatch(refreshToken))
            {
                ViewData["refreshToken"] = refreshToken;
            }
        }

        return View();
    }
}
