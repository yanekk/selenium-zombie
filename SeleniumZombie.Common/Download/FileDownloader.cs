using System.Net;
using NLog;

namespace SeleniumZombie.Common.Download
{
    public class FileDownloader : IDownloader
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Download(string fromUrl, string toPath)
        {
            _logger.Info($"Downloading {fromUrl} to {toPath}...");
            using (var client = new WebClient())
            {
                client.DownloadFile(fromUrl, toPath);
            }
        }
    }
}
