using System;

namespace SeleniumZombie.Service.Configuration
{
    public class ConfigurationModel
    {
        public TimeSpan StartTime { get; internal set; }
        public TimeSpan EndTime { get; internal set; }
        public string HubAddress { get; internal set; }
        public bool AutoUpdate { get; internal set; }
        public int ChromeInstances { get; set; }
    }
}
