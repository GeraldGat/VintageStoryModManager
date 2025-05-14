using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services.Interfaces;

namespace VintageStoryModManager.ViewModels
{
    public partial class ManageVersionPageViewModel : ObservableObject
    {
        private readonly IConfigurationService _configurationService;
        private readonly IGameVersionManager _gameVersionManager;

        [ObservableProperty]
        private ObservableCollection<VersionInfos> versions = [];

        public ManageVersionPageViewModel(IConfigurationService configurationService, IGameVersionManager gameVersionManager)
        {
            _configurationService = configurationService;
            _gameVersionManager = gameVersionManager;
            _ = LoadVersions();
        }

        private async Task LoadVersions(bool forceReload = false)
        {
            Versions.Clear();
            Versions = [.. (await _gameVersionManager.GetAvailableAndInstalledVersionsWithDownloadUrlAsync(forceReload)).Values];
        }

        [RelayCommand]
        private void ReloadVersions()
        {
            _ = LoadVersions(true);
        }

        [RelayCommand]
        private void Install(VersionInfos versionInfos)
        {
            _ = _gameVersionManager.AddGameVersion(versionInfos);
        }

        [RelayCommand]
        private void OpenFolder(VersionInfos versionInfos)
        {
            if (versionInfos.FolderName == null)
            {
                return;
            }

            string path = Path.Combine(_configurationService.AppConfig.GameVersionsPath, versionInfos.FolderName);
            if (Directory.Exists(path))
            {
                Process.Start("explorer.exe", path);
            }
            else
            {
                MessageBox.Show("Could not find the game version folder.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void Uninstall(VersionInfos versionInfos)
        {
            _ = _gameVersionManager.RemoveGameVersion(versionInfos);
        }
    }
}
