using SeleniumZombie.Common;
using SeleniumZombie.Common.Download;
using SeleniumZombie.Common.Extract;

namespace SeleniumZombie.ChromeDriver
{
    public class ChromeDriverDownloader : NewestVersionDownloader
    {
        public override string ModuleName => "ChromeDriver";

        public ChromeDriverDownloader() : base(
            new NewestVersionExtractor(new ChromeDriverVersionExtractor()), 
            new UnzipFileDownloader(), 
            "chromedriver.exe")
        {

        }
    }
}
