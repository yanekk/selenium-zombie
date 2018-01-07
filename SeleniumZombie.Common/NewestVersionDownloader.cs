using NLog;
using SeleniumZombie.Common.Download;
using SeleniumZombie.Common.Extract;
using SeleniumZombie.Common.Extract.Models;

namespace SeleniumZombie.Common
{
    public abstract class NewestVersionDownloader
    {
        public abstract string ModuleName { get; }

        private readonly INewestVersionExtractor _newestVersionExtractor;
        private readonly IDownloader _downloader;
        private readonly string _destinationFilePath;

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        protected NewestVersionDownloader(
            INewestVersionExtractor newestVersionExtractor, 
            IDownloader downloader,
            string destinationFilePath)
        {
            _newestVersionExtractor = newestVersionExtractor;
            _downloader = downloader;
            _destinationFilePath = destinationFilePath;
        }


        public void UpdateTo(VersionModel version)
        {
            _logger.Info($"Updating {ModuleName} to {version.Version}...");
            _downloader.Download(
                _newestVersionExtractor.GetNewestVersion().Url,
                CurrentDirectory.CreateFullFilePath(_destinationFilePath));
            _logger.Info($"{ModuleName} updated to {version.Version}.");
        }

        public VersionModel GetNewestVersion()
        {
            return _newestVersionExtractor.GetNewestVersion();
        }
    }
}
