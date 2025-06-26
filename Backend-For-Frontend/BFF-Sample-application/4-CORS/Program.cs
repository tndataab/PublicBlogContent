using BFFDemo_1_StartProject.Middleware;
using Duende.AccessTokenManagement.OpenIdConnect;

Console.Title = "LocalTest.me";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add HttpClient for API calls
builder.Services.AddHttpClient();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:5001")
              .AllowAnyMethod()
              .WithHeaders("X-CSRF", "Content-Type")
              .AllowCredentials()
              // Optionally cache for 10 minutes
              .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookie";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("cookie", options =>
{
    options.Cookie.Name = "__Host-AuthCookie";
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.Path = "/";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    options.SlidingExpiration = true;
    options.LoginPath = "/bff/SignInUser";
}).AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://identityservice.secure.nu";
    options.ClientId = "localhost-bff-client";                  //30 second access token
    options.ClientSecret = "mysecret";
    options.ResponseType = "code";

    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.Scope.Add("employee_info");
    options.Scope.Add("offline_access");

    options.GetClaimsFromUserInfoEndpoint = true;
    options.SaveTokens = true;
    options.Prompt = "consent";

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        NameClaimType = Duende.IdentityModel.JwtClaimTypes.Name,
        RoleClaimType = Duende.IdentityModel.JwtClaimTypes.Role
    };
});

builder.Services.AddOpenIdConnectAccessTokenManagement(o =>
{
    o.RefreshBeforeExpiration = TimeSpan.FromSeconds(15);
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors();
app.CheckForCsrfHeader();


app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

await app.RunAsync();
