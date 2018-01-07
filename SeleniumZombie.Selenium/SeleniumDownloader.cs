using SeleniumZombie.Common;
using SeleniumZombie.Common.Download;
using SeleniumZombie.Common.Extract;

namespace SeleniumZombie.Selenium
{
    public class SeleniumDownloader : NewestVersionDownloader
    {
        public override string ModuleName => "Selenium";
        private const string DestinationFileName = "selenium-server-standalone.jar";

        public SeleniumDownloader() : base(
            new NewestVersionExtractor(new SeleniumVersionExtractor()), 
            new FileDownloader(),
            DestinationFileName)
        {

        }
    }
}
