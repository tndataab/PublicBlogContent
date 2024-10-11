using IdentityServerInMem;
using IdentityService;
using Serilog;

//*************************************************************************************
// Sample Identity Server Application
// Written by Tore Nestenius
// Blog: https://nestenius.se
// Company: https://tn-data.se
// 
// Contact me if you need help or training with authentication in .NET :-)
//
// This application is not secure, it has been stripped down for educational purposes.
//*************************************************************************************

Console.Title = "IdentityService";

Serilog.Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Serilog.Log.Information("Starting up");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddRazorPages();

var isBuilder = builder.Services.AddIdentityServer(options =>
{

    options.IssuerUri = "https://localhost:7001";
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;

    // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
    options.EmitStaticAudienceClaim = true;
})
.AddTestUsers(TestUsers.Users);

// in-memory, code config
isBuilder.AddInMemoryIdentityResources(Config.IdentityResources);
isBuilder.AddInMemoryApiScopes(Config.ApiScopes);
isBuilder.AddInMemoryClients(Config.Clients);

builder.Services.AddAuthentication();

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseSerilogRequestLogging();

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();

app.MapRazorPages()
    .RequireAuthorization();

app.Run();
