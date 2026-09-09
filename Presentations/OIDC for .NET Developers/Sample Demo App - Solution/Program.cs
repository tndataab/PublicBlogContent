using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = "cookie";
    o.DefaultChallengeScheme = "oidc";
})
.AddOpenIdConnect("oidc", o =>
{
    o.Authority = "https://identityservice.secure.nu";
    o.ResponseType = "code";

    o.ClientId = "localhost-addoidc-client";
    o.ClientSecret = "mysecret";

    o.Scope.Add("openid");
    o.Scope.Add("email");
    o.Scope.Add("profile");
    o.Scope.Add("payment");
    o.MapInboundClaims = false;
    o.Prompt = "login";
    o.SaveTokens = true;
    o.PushedAuthorizationBehavior = PushedAuthorizationBehavior.Disable;
}).AddCookie("cookie");




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
