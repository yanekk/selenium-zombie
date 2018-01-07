using System;
using NLog;
using SeleniumZombie.Common;

namespace SeleniumZombie.Service.Update
{
    public class ModuleUpdater
    {
        private readonly NewestVersionDownloader[] _downloaders;
        private readonly ModuleVersionsFileManager _moduleVersionsFileManager;

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private const string VersionsFile = "versions.xml";

        public ModuleUpdater(params NewestVersionDownloader[] downloaders)
        {
            _downloaders = downloaders;
            _moduleVersionsFileManager = new ModuleVersionsFileManager(VersionsFile);
        }

        public void Update()
        {
            _logger.Info("Updating modules...");
            foreach (var downloader in _downloaders)
            {
                _logger.Info($"Updating {downloader.ModuleName}...");
                var newestVersion = downloader.GetNewestVersion();
                var installedVersion = GetInstalledVersion(downloader);
                if (newestVersion.Version <= installedVersion)
                {
                    _logger.Info($"Current version of {downloader.ModuleName} ({installedVersion}) is higher or equal to newest version ({newestVersion.Version}). No update needed.");
                    continue;
                }
                    
                _logger.Info($"Current version of {downloader.ModuleName} is lower than newest version.");
                SaveInstalledVersion(downloader, newestVersion.Version);
                downloader.UpdateTo(newestVersion);
            }
            _logger.Info("All modules are updated.");
        }

        private Version GetInstalledVersion(NewestVersionDownloader downloader)
        {
            _logger.Info($"Extracting current version of {downloader.ModuleName}...");
            var version = _moduleVersionsFileManager.GetVersion(downloader);
            return version == null ? new Version() : Version.Parse(version.Version);
        }

        private void SaveInstalledVersion(NewestVersionDownloader downloader, Version version)
        {
            _logger.Info($"Saving new version of {downloader.ModuleName}...");
            _moduleVersionsFileManager.UpdateVersion(downloader, version);
        }
    }
}
