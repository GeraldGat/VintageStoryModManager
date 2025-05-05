using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services.Interfaces;

namespace VintageStoryModManager.ViewModels
{
    public partial class SettingsPageViewModel(IConfigurationService configurationService) : ObservableObject
    {
        private readonly IConfigurationService _configurationService = configurationService;

        [ObservableProperty]
        private string modpacksPath = configurationService.AppConfig.ModpacksPath;

        [ObservableProperty]
        private string gameVersionsPath = configurationService.AppConfig.GameVersionsPath;

        [RelayCommand]
        private void BrowseGameVersionsPath()
        {
            var dialog = new OpenFolderDialog();
            if (dialog.ShowDialog() == true)
            {
                GameVersionsPath = dialog.FolderName;
                _configurationService.AppConfig.GameVersionsPath = dialog.FolderName;
                _configurationService.SaveConfiguration();
            }
        }

        [RelayCommand]
        private void ResetGameVersionsPath()
        {
            GameVersionsPath = AppConfig.DefaultGameVersionPath();
            _configurationService.AppConfig.GameVersionsPath = GameVersionsPath;
            _configurationService.SaveConfiguration();
        }

        [RelayCommand]
        private void BrowseModpacksPath()
        {
            var dialog = new OpenFolderDialog();
            if (dialog.ShowDialog() == true)
            {
                ModpacksPath = dialog.FolderName;
                _configurationService.AppConfig.ModpacksPath = dialog.FolderName;
                _configurationService.SaveConfiguration();
            }
        }

        [RelayCommand]
        private void ResetModpacksPath()
        {
            ModpacksPath = AppConfig.DefaultModpackPath();
            _configurationService.AppConfig.ModpacksPath = ModpacksPath;
            _configurationService.SaveConfiguration();
        }
    }
}
