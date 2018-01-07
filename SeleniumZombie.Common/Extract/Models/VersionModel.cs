using System;

namespace SeleniumZombie.Common.Extract.Models
{
    public class VersionModel
    {
        public Version Version { get; set; } 
        public string Url { get; set; }

        public VersionModel(Version version, string url)
        {
            Version = version;
            Url = url;
        }

        public override string ToString()
        {
            return $"Version: {Version}, Url: {Url}";
        }
    }
}
