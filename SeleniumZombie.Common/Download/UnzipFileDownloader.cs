using System.IO;
using System.Linq;
using NLog;

namespace SeleniumZombie.Common.Download
{
    public class UnzipFileDownloader : IDownloader
    {
        private readonly FileDownloader _fileDownloader;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public UnzipFileDownloader()
        {
            _fileDownloader = new FileDownloader();
        }

        public void Download(string fromUrl, string toPath)
        {
            var zipFilePath = toPath + ".zip";
            var zipFileDirectory = Path.GetFileNameWithoutExtension(toPath) + ".temp";

            _fileDownloader.Download(fromUrl, zipFilePath);
            _logger.Info($"Unzipping {zipFilePath} to {zipFileDirectory}...");
            Directory.CreateDirectory(zipFileDirectory);

            System.IO.Compression.ZipFile.ExtractToDirectory(zipFilePath, zipFileDirectory);

            var file = Directory.GetFiles(zipFileDirectory).First();
            _logger.Info($"Copying {file} to {toPath}...");
            File.Copy(file, toPath, true);

            _logger.Info($"Deleting {zipFileDirectory}.");
            Directory.Delete(zipFileDirectory, true);
            File.Delete(zipFilePath);
        }
    }
}
