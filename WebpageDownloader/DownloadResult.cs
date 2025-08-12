namespace WebpageDownloader.App
{
    public class DownloadResult
    {
        public bool Success { get; set; }
        public string? Content { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
