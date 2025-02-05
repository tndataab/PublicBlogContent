using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Demo_application___Base_code.Controllers;

// URL: /Secure/Index
public class SecureController : Controller
{
    public async Task<IActionResult> Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            await HttpContext.ChallengeAsync("cookie");
        }

        return View();
    }
}

