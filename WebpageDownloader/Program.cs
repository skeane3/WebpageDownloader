using Microsoft.Extensions.Configuration;
using Serilog;

namespace WebpageDownloader.App
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            // Build configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Configure Serilog for logging
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            Log.Information("Application started.");

            Console.WriteLine("Enter URLs separated by commas:");
            string input = Console.ReadLine();
            var urls = input.Split(',');

            Log.Information($"Received {urls.Length} URLs.");

            var downloader = new Downloader(Log.Logger);
            var results = await downloader.DownloadPagesAsync(urls);

            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].Success)
                {
                    string fileName = $"Page_{i + 1}.html";
                    await File.WriteAllTextAsync(fileName, results[i].Content);
                    Log.Information($"Saved {urls[i].Trim()} to {fileName}");
                }
                else
                {
                    Log.Error($"Failed to download {urls[i].Trim()}: {results[i].ErrorMessage}");
                }
            }

            Console.WriteLine("Download complete! Check logs for details.");
            Log.Information("Application finished.");
        }
    }
}