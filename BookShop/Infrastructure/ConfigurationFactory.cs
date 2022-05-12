using Microsoft.Extensions.Configuration;
using System;
using System.Text.RegularExpressions;

namespace BookShop.Infrastructure
{
    public class ConfigurationFactory
    {
        protected IConfigurationBuilder ConfigurationBuilder { get; set; }

        public ConfigurationFactory()
        {
            string currentProjectDirectory = ParseProjectDictionaryPath();
            if (currentProjectDirectory == "")
                ConfigurationBuilder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json");
            else
                ConfigurationBuilder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .SetBasePath(currentProjectDirectory);
        }

        protected string ParseProjectDictionaryPath()
        {
            var match = Regex.Match(AppDomain.CurrentDomain.BaseDirectory, @"^(.+?\\)bin");
            if (!match.Success) return "";
            return match.Groups[1].Value;
        }

        public IConfigurationRoot GetConfiguration()
        {
            return ConfigurationBuilder.Build();
        }
    }
}
