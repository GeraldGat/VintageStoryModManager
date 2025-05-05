using System.IO;
using VintageStoryModManager.Services;

namespace VintageStoryModManager.Models
{
    public class AppConfig
    {
        public string GameVersionsPath { get; set; } = DefaultGameVersionPath();
        public string ModpacksPath { get; set; } = DefaultGameVersionPath();
        public string AppTheme { get; set; } = ThemeManager.GetAppColorThemeFromSystem();

        public static string DefaultGameVersionPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GamesVersions"); ;
        }

        public static string DefaultModpackPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modpacks");
        }
    }
}
