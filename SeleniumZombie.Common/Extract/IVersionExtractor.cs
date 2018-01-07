using SeleniumZombie.Common.Extract.Models;

namespace SeleniumZombie.Common.Extract
{
    public  interface IVersionExtractor
    {
        VersionModel[] GetAvailableVersions();
    }
}
