using Duende.Bff.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFFDemo_1_StartProject.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    [BffApi]
    public class ApiController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ApiController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("local")]
        public IActionResult Local()
        {
            var response = new
            {
                message = $"Local API Was called at {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                timestamp = DateTime.Now
            };

            return Ok(response);
        }
    }
}