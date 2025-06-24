using Duende.AccessTokenManagement.OpenIdConnect;
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
        private readonly IUserTokenManagementService _tokenManagement;

        public ApiController(HttpClient httpClient, IUserTokenManagementService tokenManagement)
        {
            _httpClient = httpClient;
            _tokenManagement = tokenManagement;
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
                // Get access token using Duende Access Token Management
                // (handles automatic refresh)
                var tokenResult = await _tokenManagement.GetAccessTokenAsync(User);
                if (tokenResult.IsError)

                {
                    return StatusCode((int)HttpStatusCode.Unauthorized, new
                    {
                        error = "Failed to get access token",
                        details = $"Token error: {tokenResult.Error}",
                        statusCode = 401,
                        tokenManagementUsed = true
                    });
                }

                // Create HTTP request with Authorization header
                var url = "https://www.secure.nu/tokenapi/gettime";
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                var authHeader = new AuthenticationHeaderValue("Bearer", tokenResult.AccessToken);
                request.Headers.Authorization = authHeader;

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