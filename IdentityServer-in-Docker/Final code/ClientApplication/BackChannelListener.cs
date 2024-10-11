using Serilog;
using System.Diagnostics;

namespace ClientApplication;


/// <summary>
/// Backchannel listener, that will log requests made to our IdentityServer 
/// Taken from the IdentityServer in production training class https://www.tn-data.se
/// </summary>
public class BackChannelListener : DelegatingHandler
{
    public BackChannelListener() : base(new HttpClientHandler())
    {
    }

    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var sw = new Stopwatch();
        sw.Start();

        var response = await base.SendAsync(request, cancellationToken);

        sw.Stop();

        // Read the response content (make sure to await it)
        var responseContent = await response.Content.ReadAsStringAsync();

        // HACK: log the response body; Never run this in production
        Log.Logger.ForContext("SourceContext", "BackChannelListener")
        .Information("#####################################");
        Log.Logger.ForContext("SourceContext", "BackChannelListener")
        .Information(responseContent);
        Log.Logger.ForContext("SourceContext", "BackChannelListener")
        .Information("#####################################");

        Log.Logger.ForContext("SourceContext", "BackChannelListener")
                   .Information($"### BackChannel request to {request?.RequestUri?.AbsoluteUri} took {sw.ElapsedMilliseconds.ToString()} ms");

        return response;
    }
}