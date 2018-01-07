using System.Linq;
using NLog;
using SeleniumZombie.Common.Extract;
using SeleniumZombie.Common.Extract.Models;

namespace SeleniumZombie.ChromeDriver
{
    internal class ChromeDriverVersionExtractor : IVersionExtractor
    {
        private readonly VersionExtractor _versionExtractor;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ChromeDriverVersionExtractor()
        {
            _versionExtractor = new VersionExtractor("http://chromedriver.storage.googleapis.com");
        }

        public VersionModel[] GetAvailableVersions()
        {
            _logger.Info("Getting newest chromedriver version...");
            return _versionExtractor.GetAvailableVersions()
                .Where(model => model.Url.Contains("chromedriver_win32.zip"))
                .ToArray();
        }
    }
}
