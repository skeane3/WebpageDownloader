using WebpageDownloader.App;
using Moq;
using Serilog;

namespace WebpageDownloader.Tests
{
    public class WebpageDownloaderTests
    {
        private Mock<ILogger> _mockLogger;

        [SetUp]
        public void SetUp()
        {
            // Initialize the mock logger before each test
            _mockLogger = new Mock<ILogger>();
        }

        [Test]
        public async Task DownloadPagesAsync_ValidUrls_ReturnsContent()
        {
            // Arrange
            var downloader = new Downloader(_mockLogger.Object);
            var urls = new[] { "https://www.google.com" };

            // Act
            var results = await downloader.DownloadPagesAsync(urls);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(urls.Length, Is.EqualTo(results.Count));

                Assert.That(results[0].Success, Is.EqualTo(true));
                Assert.That(results[0].Content, Does.Contain("<html"));
                Assert.That(results[0].ErrorMessage, Is.EqualTo(null));


                Assert.That(results[1].Success, Is.EqualTo(true));
                Assert.That(results[1].Content, Does.Contain("<html"));
                Assert.That(results[1].ErrorMessage, Is.EqualTo(null));
            });
        }

        [Test]
        public async Task DownloadPagesAsync_InvalidUrl_ReturnsErrorMessage()
        {
            // Arrange
            var downloader = new Downloader(_mockLogger.Object);
            var urls = new[] { "invalid-url" };

            // Act
            var results = await downloader.DownloadPagesAsync(urls);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(urls.Length, Is.EqualTo(results.Count));
                Assert.That(results[0].Success, Is.EqualTo(false));
                Assert.That(results[0].Content, Is.EqualTo(null));
                Assert.That(results[0].ErrorMessage, Does.Contain("An invalid request URI was provided"));
            });
        }
    }
}