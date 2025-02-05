using System.Diagnostics;

public class BackChannelRetryHandler : DelegatingHandler
{
    private static Stopwatch globalsw = new Stopwatch();
    private static Stopwatch diffsw = new Stopwatch();


    public BackChannelRetryHandler() : base(new HttpClientHandler())
    {
        WriteToLog("##################### START #############################");
    }

    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken token)
    {
        if (!globalsw.IsRunning)
            globalsw.Start();



        string message = globalsw.Elapsed.ToString(@"mm\:ss");

        if (!diffsw.IsRunning)
        {
            message = message + $"              ";
            diffsw.Start();
        }
        else
        {
            message = message + $"   Diff: {diffsw.Elapsed.ToString(@"mm\:ss")}";
            diffsw.Restart();
        }

        var url = request?.RequestUri?.AbsoluteUri;

        WriteToLog(message + $"       - {url}");

        var response = await base.SendAsync(request, token);
        return response;
    }


    private static void WriteToLog(string message)
    {
        using (StreamWriter w = File.AppendText("BackChannelRequestLog.txt"))
        {
            Console.WriteLine(message);
            w.WriteLine(message);
        }
    }
}