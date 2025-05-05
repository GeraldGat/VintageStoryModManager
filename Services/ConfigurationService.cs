using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.Json;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services.Interfaces;

namespace VintageStoryModManager.Services
{
    internal class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;

        public AppConfig AppConfig { get; private set; }

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
            AppConfig = _configuration.GetSection("AppConfig").Get<AppConfig>() ?? new();
        }

        public void SaveConfiguration()
        {
            var newConfig = new {
                AppConfig = AppConfig
            };
            string json = JsonSerializer.Serialize(newConfig, new JsonSerializerOptions());
            File.WriteAllText(App.AppSettingsPath, json);
        }
    }
}
