using Microsoft.AspNetCore.Mvc;

namespace OIDC_client.Controllers;

// URL: /Secure/Index
public class SecureController : Controller
{
    public async Task<IActionResult> Index()
    {
        if (User.Identity.IsAuthenticated == false)
            return Challenge();
        else
            return View();
    }

}

