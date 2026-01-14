using IdentityServerHost;
using IdentityServerInMem;
using Serilog;
using Serilog.Events;
using System.Globalization;

Console.Title = "IdentityService";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting host...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddRazorPages();

    builder.Services.AddSerilog((services, lc) =>
    {
        lc.MinimumLevel.Information()
          .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
          .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
          .MinimumLevel.Override("System", LogEventLevel.Warning)
          .MinimumLevel.Override("Duende", LogEventLevel.Debug)
          .ReadFrom.Services(services)
          .Enrich.FromLogContext()
          .WriteTo.Console();
    });

    builder.Services.AddAuthentication();
    builder.Services.AddAuthorization();

    var isBuilder = builder.Services.AddIdentityServer(options =>
    {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;
    }).AddTestUsers(TestUsers.Users);

    // in-memory, code config
    isBuilder.AddInMemoryIdentityResources(Config.IdentityResources);
    isBuilder.AddInMemoryApiScopes(Config.ApiScopes);
    isBuilder.AddInMemoryClients(Config.Clients);


    var app = builder.Build();

    app.UseStaticFiles();
    app.UseSerilogRequestLogging();

    app.UseRouting();

    app.UseIdentityServer();

    app.UseAuthorization();

    app.MapRazorPages()
        .RequireAuthorization();

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}