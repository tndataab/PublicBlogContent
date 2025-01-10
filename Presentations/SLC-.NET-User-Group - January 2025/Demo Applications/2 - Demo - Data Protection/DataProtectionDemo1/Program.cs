using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddControllersWithViews();

//******************************************************


var keyPath = new DirectoryInfo(@"C:\Conf\Keys");

builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(keyPath)
                .SetApplicationName("MyApplication");



//******************************************************


builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = "cookie";
})
.AddCookie("cookie", o =>
{
    o.LoginPath = "/SimpleLogin/Login";
    o.LogoutPath = "/SimpleLogin/Logout";
});


var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
