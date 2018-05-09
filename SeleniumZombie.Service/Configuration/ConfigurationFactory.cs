using System;
using System.Configuration;

namespace SeleniumZombie.Service.Configuration
{
    public static class ConfigurationFactory
    {
        public static ConfigurationModel Create()
        {
            var startTime = TimeSpan.Parse(GetConfigurationSetting("StartTime"));
            var endTime = TimeSpan.Parse(GetConfigurationSetting("EndTime"));
            var hubAddress = GetConfigurationSetting("HubAddress");
            var autoUpdate = bool.Parse(GetConfigurationSetting("AutoUpdate"));
            var chromeInstances = int.Parse(GetConfigurationSetting("ChromeInstances"));
            var host = GetConfigurationSetting("Host");

            return new ConfigurationModel
            {
                StartTime = startTime,
                EndTime = endTime,
                HubAddress = hubAddress,
                AutoUpdate = autoUpdate,
                ChromeInstances = chromeInstances,
                Host = host
            };
        }

        private static string GetConfigurationSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
