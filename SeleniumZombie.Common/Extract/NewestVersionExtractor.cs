using System.Linq;
using SeleniumZombie.Common.Extract.Models;

namespace SeleniumZombie.Common.Extract
{
    public class NewestVersionExtractor : INewestVersionExtractor
    {
        private readonly IVersionExtractor _versionExtractor;

        public NewestVersionExtractor(IVersionExtractor versionExtractor)
        {
            _versionExtractor = versionExtractor;
        }
        
        public VersionModel GetNewestVersion()
        {
            return _versionExtractor
                .GetAvailableVersions()
                .OrderByDescending(v => v.Version)
                .First();
        }
    }
}
