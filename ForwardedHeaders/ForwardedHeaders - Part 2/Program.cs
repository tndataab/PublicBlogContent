using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using System.Text.Json;

// Code for the Forwarded Headers blog post on https://nestenius.se
// For more details about my consulting and training services, visit
// https://tn-data.se 
//
// This project covers:
// * ASPNETCORE_FORWARDEDHEADERS_ENABLED environment variable
// * The FORWARDEDHEADERS_ENABLED configuration setting
//

var builder = WebApplication.CreateBuilder(args);

// Use the settings in appsettings.json
builder.Services.Configure<ForwardedHeadersOptions>(builder.Configuration.GetSection("ForwardedHeaders"));


//For manual configuration
//builder.Services.Configure<ForwardedHeadersOptions>(options =>
//{
//    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
//                               ForwardedHeaders.XForwardedHost |
//                               ForwardedHeaders.XForwardedProto;
//});


var app = builder.Build();


app.MapGet("{**route}", async (HttpContext context,
                              IOptions<ForwardedHeadersOptions> forwardedOptions) =>
{
    var requestDetails = new
    {
        Protocol = context.Request.Protocol,
        Scheme = context.Request.Scheme,
        Path = context.Request.Path.Value,
        PathBase = context.Request.PathBase.Value,
        Host = context.Request.Host.ToString(),
        DisplayUrl = context.Request.GetDisplayUrl(),
        RemoteIpAddress = context.Connection.RemoteIpAddress?.ToString(),
        Headers = context.Request.Headers
                  .ToDictionary(h => h.Key, h => h.Value.ToString()),

        ForwardedHeaders = new
        {
            ForwardedHeaders = GetEnabledFlags(forwardedOptions.Value.ForwardedHeaders),
            AllowedHosts = forwardedOptions.Value.AllowedHosts.ToList(),
            KnownNetworks = forwardedOptions.Value.KnownNetworks
                            .Select(n => n.Prefix + "/" + n.PrefixLength)
                            .ToList(),
            KnownProxies = forwardedOptions.Value.KnownProxies
                            .Select(p => p.ToString()).ToList(),
            forwardedOptions.Value.ForwardLimit,
            forwardedOptions.Value.RequireHeaderSymmetry,
            forwardedOptions.Value.OriginalForHeaderName,
            forwardedOptions.Value.OriginalHostHeaderName,
            forwardedOptions.Value.OriginalProtoHeaderName,
            forwardedOptions.Value.OriginalPrefixHeaderName,
            forwardedOptions.Value.ForwardedForHeaderName,
            forwardedOptions.Value.ForwardedHostHeaderName,
            forwardedOptions.Value.ForwardedProtoHeaderName,
            forwardedOptions.Value.ForwardedPrefixHeaderName
        }
    };

    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync(JsonSerializer.Serialize(requestDetails));
});



app.Run();




static List<string> GetEnabledFlags(ForwardedHeaders headers)
{
    return Enum.GetValues<ForwardedHeaders>()
        .Where(flag => flag != ForwardedHeaders.None && headers.HasFlag(flag))
        .Select(flag => flag.ToString())
        .ToList();
}