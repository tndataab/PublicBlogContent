using System.Diagnostics;

public class BackChannelRetryHandler : DelegatingHandler
{
    private DateTime? lastRequestTime = null;
    private readonly Stopwatch globalTimer = new Stopwatch();

    public BackChannelRetryHandler() : base(new HttpClientHandler())
    {
        WriteToLog("##################### START #############################");
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken token)
    {
        if (!globalTimer.IsRunning)
            globalTimer.Start();

        var now = DateTime.UtcNow;
        var sinceStart = globalTimer.Elapsed.ToString(@"mm\:ss");
        string diff = "";

        if (lastRequestTime.HasValue)
        {
            var timeDiff = now - lastRequestTime.Value;
            diff = $"   Diff: {timeDiff:mm\\:ss}";
        }
        else
        {
            diff = $"              ";
        }

        lastRequestTime = now;

        var url = request?.RequestUri?.AbsoluteUri;
        WriteToLog($"{sinceStart}{diff}   - {url}");

        return await base.SendAsync(request, token);
    }

    private static void WriteToLog(string message)
    {
        Console.WriteLine(message);
    }
}