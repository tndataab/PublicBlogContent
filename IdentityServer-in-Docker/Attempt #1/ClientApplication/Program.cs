using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;

//************************************************************************************
// Sample Client Application
// Written by Tore Nestenius
// Blog: https://nestenius.se
// Company: https://tn-data.se
// 
// Contact me if you need help or training with authentication in .NET :-)
//
// This application is not secure, it has been stripped down for educational purposes.
//*************************************************************************************

Console.Title = "Client Application";

IdentityModelEventSource.ShowPII = true;

Serilog.Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
}).AddCookie(opt =>
{
    opt.LogoutPath = "/user/Logout";
    opt.AccessDeniedPath = "/user/AccessDenied";
    opt.SlidingExpiration = true;

}).AddOpenIdConnect(options =>
{
    options.Authority = "http://localhost:7000";

    options.ClientId = "localhost-addoidc-client";

    options.ClientSecret = "mysecret";
    options.ResponseType = "code";

    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.Scope.Add("offline_access");

    options.GetClaimsFromUserInfoEndpoint = true;
    options.SaveTokens = true;
    options.RequireHttpsMetadata = false;

    options.AccessDeniedPath = "/User/AccessDenied";

    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = JwtClaimTypes.Name,
        RoleClaimType = JwtClaimTypes.Role
    };
});

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseSerilogRequestLogging();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
