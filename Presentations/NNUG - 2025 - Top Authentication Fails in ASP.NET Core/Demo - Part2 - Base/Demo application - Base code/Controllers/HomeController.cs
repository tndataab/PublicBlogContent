using Demo_application___Base_code.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Demo_application___Base_code.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Session()
    {
        var counterString = HttpContext.Session.GetString("Counter");
        var counter = string.IsNullOrEmpty(counterString) ? 0 : int.Parse(counterString);
        counter++;
        HttpContext.Session.SetString("Counter", counter.ToString());

        HttpContext.Session.SetString("Favourite blog", "https://nestenius.se");

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
