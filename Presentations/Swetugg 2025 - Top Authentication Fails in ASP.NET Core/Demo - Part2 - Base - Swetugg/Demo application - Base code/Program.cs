using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Collections.Concurrent;
using Demo_application___Base_code;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore.DataProtection", Serilog.Events.LogEventLevel.Error)
    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", Serilog.Events.LogEventLevel.Error)
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Error)
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(o =>
{
    o.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
    o.Cookie.SameSite = SameSiteMode.Strict;
});

builder.Services.AddHttpContextAccessor();


//******************************************************************

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = "cookie";
    o.DefaultChallengeScheme = "oidc";
})
.AddCookie("cookie", o =>
{
    o.LoginPath = "/SimpleLogin/Login";
    o.LogoutPath = "/user/Logout";
    o.AccessDeniedPath = "/user/AccessDenied";

    o.DataProtectionProvider = new MyDataProtector();
    o.SessionStore = new AdvancedSessionStore();



}).AddOpenIdConnect("oidc", o =>
{
    o.Authority = "https://identityservice.secure.nu";
    o.ClientId = "localhost-addoidc-client";

    o.ClientSecret = "mysecret";
    o.ResponseType = "code";

    o.Scope.Clear();
    o.Scope.Add("openid");
    o.Scope.Add("profile");
    o.Scope.Add("email");
    o.Scope.Add("employee_info");
    o.Scope.Add("offline_access");

    o.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = JwtClaimTypes.Name,
        RoleClaimType = JwtClaimTypes.Role,
    };

    o.GetClaimsFromUserInfoEndpoint = true;
    o.SaveTokens = true;

    o.PushedAuthorizationBehavior = PushedAuthorizationBehavior.Disable;

    o.SignedOutRedirectUri = "/";
});

//******************************************************************



builder.Host.UseSerilog();

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


await app.RunAsync();



public class MyDataProtector : IDataProtector
{
    public IDataProtector CreateProtector(string purpose)
    {
        return new MyDataProtector();
    }
    public byte[] Protect(byte[] plaintext)
    {
        return plaintext;
    }
    public byte[] Unprotect(byte[] protectedData)
    {
        return protectedData;
    }
}


public class MySessionStore : ITicketStore
{
    private readonly ConcurrentDictionary<string, AuthenticationTicket> mytickets = new();

    public async Task RemoveAsync(string key)
    {
        if (mytickets.ContainsKey(key))
            mytickets.TryRemove(key, out _);
    }

    public async Task RenewAsync(string key, AuthenticationTicket ticket)
    {
        mytickets[key] = ticket;
    }

    public async Task<AuthenticationTicket> RetrieveAsync(string key)
    {
        return mytickets.TryGetValue(key, out var ticket) ? ticket : default;
    }

    public async Task<string> StoreAsync(AuthenticationTicket ticket)
    {
        var key = Guid.NewGuid().ToString();
        if (mytickets.TryAdd(key, ticket))
            return key;
        else
            throw new Exception("Failed to add entry to MySessionStore");
    }
}
