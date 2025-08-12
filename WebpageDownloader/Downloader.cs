using Serilog;

namespace WebpageDownloader.App
{
    public class Downloader
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public Downloader(ILogger logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public async Task<List<DownloadResult>> DownloadPagesAsync(string[] urls)
        {
            var tasks = new List<Task<DownloadResult>>(urls.Length);

            foreach (var url in urls)
            {
                tasks.Add(DownloadPageAsync(url.Trim()));
            }

            return [.. await Task.WhenAll(tasks)];
        }

        private async Task<DownloadResult> DownloadPageAsync(string url)
        {
            try
            {
                _logger.Information($"Downloading {url}...");
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return new DownloadResult { Success = true, Content = content };
            }
            catch (Exception ex)
            {
                _logger.Error($"Error downloading {url}: {ex.Message}");
                return new DownloadResult { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
