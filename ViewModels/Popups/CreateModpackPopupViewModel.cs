using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services.Interfaces;

namespace VintageStoryModManager.ViewModels.Popups
{
    public partial class CreateModpackPopupViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private readonly IGameVersionManager _gameVersionManager;

        [ObservableProperty]
        private string modpackName = string.Empty;

        [ObservableProperty]
        private ObservableCollection<VersionInfos> installedVersions = [];

        [ObservableProperty]
        private VersionInfos? selectedVersion;

        public CreateModpackPopupViewModel(IDialogService dialogService, IGameVersionManager gameVersionManager)
        {
            _dialogService = dialogService;
            _gameVersionManager = gameVersionManager;
            _ = LoadInstalledVersion();
        }

        private async Task LoadInstalledVersion()
        {
            InstalledVersions.Clear();
            InstalledVersions = [.. (await _gameVersionManager.GetInstalledVersions()).Values];
        }

        [RelayCommand]
        private void Create()
        {
            if (string.IsNullOrWhiteSpace(ModpackName))
            {
                MessageBox.Show("Please enter a modpack name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (SelectedVersion == null)
            {
                MessageBox.Show("Please select a version.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _dialogService.CloseDialog(true);
        }

        [RelayCommand]
        private void Cancel()
        {
            _dialogService.CloseDialog(false);
        }
    }
}
