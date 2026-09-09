using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Starter_Project.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Starter_Project.Controllers;

public class PaymentApiController : Controller
{
    private const string PaymentsEndpoint = "https://paymentapi.secure.nu/payments";

    public IActionResult Index()
    {
        return View(new PaymentApiModel());
    }

    [HttpPost]
    public async Task<IActionResult> GetPayments()
    {
        var model = new PaymentApiModel();

        model.AccessToken = await HttpContext.GetTokenAsync("access_token") ?? "";

        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", model.AccessToken);

        var response = await client.GetAsync(PaymentsEndpoint);

        model.Result = await FormatResponseAsync(response);

        return View("Index", model);
    }


    private static async Task<string> FormatResponseAsync(HttpResponseMessage response)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"HTTP {(int)response.StatusCode} {response.ReasonPhrase}");

        foreach (var header in response.Headers.Concat(response.Content.Headers))
        {
            sb.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
        }

        string body = await response.Content.ReadAsStringAsync();

        if (!string.IsNullOrWhiteSpace(body))
        {
            sb.AppendLine();
            sb.Append(BeautifyJson(body));
        }

        return sb.ToString();
    }

    private static string BeautifyJson(string body)
    {
        try
        {
            using var document = JsonDocument.Parse(body);
            return JsonSerializer.Serialize(document.RootElement, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (JsonException)
        {
            return body;
        }
    }
}
