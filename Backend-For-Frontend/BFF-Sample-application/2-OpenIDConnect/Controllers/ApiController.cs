using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;

namespace BFFDemo_1_StartProject.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
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

        [HttpGet("remote")]
        public async Task<IActionResult> Remote()
        {
            try
            {
                // Get access token from OIDC authentication
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                if (string.IsNullOrEmpty(accessToken))
                {
                    return StatusCode((int)HttpStatusCode.Unauthorized, new
                    {
                        error = "No access token available",
                        details = "User must be authenticated with OIDC to access remote API",
                        statusCode = 401
                    });
                }

                // Create HTTP request with Authorization header to our remote API
                // Important, you need to update the URL to match your own remote API endpoint, the secure.nu domain is not always available
                var request = new HttpRequestMessage(HttpMethod.Get, "https://www.secure.nu/tokenapi/gettime");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Send request with token
                var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new
                    {
                        message = "Remote API call successful with access token",
                        data = content,
                        statusCode = (int)response.StatusCode,
                    });
                }
                else
                {
                    return StatusCode((int)response.StatusCode, new
                    {
                        error = $"Remote API returned error: {response.StatusCode}",
                        details = content,
                        statusCode = (int)response.StatusCode
                    });
                }
            }
            catch (HttpRequestException ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway, new
                {
                    error = "Failed to connect to remote API",
                    details = ex.Message,
                    statusCode = 502
                });
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout, new
                {
                    error = "Remote API request timed out",
                    details = ex.Message,
                    statusCode = 408
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    error = "An unexpected error occurred",
                    details = ex.Message,
                    statusCode = 500
                });
            }
        }
    }
}