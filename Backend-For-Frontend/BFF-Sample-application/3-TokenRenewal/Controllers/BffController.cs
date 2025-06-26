using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BFFDemo_1_StartProject.Controllers
{
    public class BffController : Controller
    {
        /// <summary>
        /// Challenge the user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task Login()
        {
            await HttpContext.ChallengeAsync("oidc", new AuthenticationProperties { RedirectUri = "/" });
        }

        /// <summary>
        /// Sign-out the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Logout()
        {
            return SignOut(
                new AuthenticationProperties
                {
                    RedirectUri = "/"
                },
                "cookie",
                "oidc");
        }

        [HttpPost]
        public async Task<IActionResult> Session()
        {
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                return Json(new
                {
                    isAuthenticated = false,
                    message = "User is not authenticated"
                });
            }

            // Get authentication result to access properties
            var authResult = await HttpContext.AuthenticateAsync("cookie");

            // Extract user claims
            var claims = User.Claims.Select(c => new
            {
                type = c.Type,
                value = c.Value
            }).ToArray();

            // Extract identity information
            var identity = new
            {
                authenticationType = User.Identity?.AuthenticationType ?? "",
                isAuthenticated = User.Identity?.IsAuthenticated ?? false,
                name = User.Identity?.Name ?? "[Unknown]"
            };

            // Extract authentication properties
            var authProperties = new Dictionary<string, object>();
            if (authResult.Properties != null)
            {
                // Add standard properties
                if (authResult.Properties.IssuedUtc.HasValue)
                    authProperties["issuedUtc"] = authResult.Properties.IssuedUtc.Value;

                if (authResult.Properties.ExpiresUtc.HasValue)
                    authProperties["expiresUtc"] = authResult.Properties.ExpiresUtc.Value;

                if (!string.IsNullOrEmpty(authResult.Properties.RedirectUri))
                    authProperties["redirectUri"] = authResult.Properties.RedirectUri;

                // Add custom items
                foreach (var item in authResult.Properties.Items)
                {
                    authProperties[item.Key] = item.Value;
                }
            }

            // Calculate cookie lifetime information
            TimeSpan? remainingTime = null;
            if (authResult.Properties?.ExpiresUtc.HasValue == true)
            {
                remainingTime = authResult.Properties.ExpiresUtc.Value - DateTimeOffset.UtcNow;
            }

            var cookieLifetime = new
            {
                issuedUtc = authResult.Properties?.IssuedUtc,
                expiresUtc = authResult.Properties?.ExpiresUtc,
                remainingTime = remainingTime,
            };

            // Create comprehensive response
            var sessionInfo = new
            {
                timestamp = DateTimeOffset.UtcNow,
                claims = claims,
                identity = identity,
                authenticationProperties = authProperties,
                cookieLifetime = cookieLifetime
            };

            return Json(sessionInfo, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
    }
}