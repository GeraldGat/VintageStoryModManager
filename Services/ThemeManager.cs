using Microsoft.Win32;
using System.Windows;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services.Interfaces;

namespace VintageStoryModManager.Services
{
    internal class ThemeManager : IThemeManager
    {
        public void ApplyTheme(string theme)
        {
            var dictUri = new Uri($"/Resources/Styles/Themes/{theme}", UriKind.Relative);
            var newDict = new ResourceDictionary() { Source = dictUri };

            var appResources = Application.Current.Resources.MergedDictionaries;

            var existingTheme = appResources.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Theme.xaml"));
            if (existingTheme != null)
            {
                appResources.Remove(existingTheme);
            }

            appResources.Add(newDict);
        }

        public static string GetAppColorThemeFromSystem()
        {
            const string keyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(keyPath);

            if (key?.GetValue("AppsUseLightTheme") is int value)
            {
                return value == 0 ? AppTheme.Dark : AppTheme.Dark;
            }

            return AppTheme.Dark;
        }
    }
}
