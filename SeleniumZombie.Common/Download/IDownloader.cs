namespace SeleniumZombie.Common.Download
{
    public interface IDownloader
    {
        void Download(string fromUrl, string toPath);
    }
}
