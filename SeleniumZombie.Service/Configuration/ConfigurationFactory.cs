using System;
using System.Configuration;

namespace SeleniumZombie.Service.Configuration
{
    public static class ConfigurationFactory
    {
        public static ConfigurationModel Create()
        {
            var startTime = TimeSpan.Parse(ConfigurationManager.AppSettings["StartTime"]);
            var endTime = TimeSpan.Parse(ConfigurationManager.AppSettings["EndTime"]);
            var hubAddress = ConfigurationManager.AppSettings["HubAddress"];
            var autoUpdate = bool.Parse(ConfigurationManager.AppSettings["AutoUpdate"]);

            return new ConfigurationModel
            {
                StartTime = startTime,
                EndTime = endTime,
                HubAddress = hubAddress,
                AutoUpdate = autoUpdate
            };
        }
    }
}
