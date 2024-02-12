using Azure.Identity;
using Microsoft.AspNetCore.DataProtection;
using Serilog;
using Serilog.Events;
using TnData.AzureKeyVaultExtensions;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .MinimumLevel.Override("Microsoft.AspNetCore.DataProtection", LogEventLevel.Verbose)
    .MinimumLevel.Override("TnData.AzureKeyVaultExtensions", LogEventLevel.Verbose)
    .WriteTo.Console()
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDataProtection()
                .SetApplicationName("MyBusinessApplication")
                .PersistKeysToAzureKeyVault(new DefaultAzureCredential(),
                                            vaultUri: new Uri("https://dpapikeyvault.vault.azure.net/"),
                                            keyRingName: "MyKeyRing");


builder.Services.AddSerilog();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
