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

//Example of using Azure Key Vault to both store the key ring and the encryption key
builder.Services.AddDataProtection()
                .SetApplicationName("MyBusinessApplication")
                .ProtectKeysWithAzureKeyVault(new Uri("https://dpapikeyvault.vault.azure.net/keys/MyDPAPIKey/c07cda675a3446f6a752a1e50c17c0c7"), new DefaultAzureCredential())
                .PersistKeysToAzureKeyVault(new DefaultAzureCredential(),
                                            vaultUri: new Uri("https://dpapikeyvault.vault.azure.net/"),
                                            keyRingName: "MyKeyRing2");

// Example of just using the default developer setup
//builder.Services.AddDataProtection();



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