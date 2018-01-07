using System;
using System.Linq;
using NLog;
using SeleniumZombie.Common.Extract;
using SeleniumZombie.Common.Extract.Models;

namespace SeleniumZombie.Selenium
{
    internal class SeleniumVersionExtractor : IVersionExtractor
    {
        private readonly VersionExtractor _versionExtractor;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public SeleniumVersionExtractor()
        {
            _versionExtractor = new VersionExtractor("http://selenium-release.storage.googleapis.com");
            _versionExtractor.AddFilter((version, url) => url.Contains("selenium-server-standalone") && !url.Contains("beta"));
        }

        public VersionModel[] GetAvailableVersions()
        {
            _logger.Info("Getting newest Selenium version...");
            var versions = _versionExtractor
                .GetAvailableVersions()
                .ToArray();

            foreach (var version in versions)
            {
                var sameVersions = versions
                    .Where(v => v.Version == version.Version)
                    .Skip(1)
                    .ToArray();

                for (var i = 0; i < sameVersions.Length; i++)
                {
                    var newVersion = sameVersions[i].Version + "." + (i+1);
                    sameVersions[i].Version = Version.Parse(newVersion);
                }
            }
            return versions;
        }
    }
}
