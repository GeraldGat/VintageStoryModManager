using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.IO;
using System.Text.Json;
using System.Windows;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services.Interfaces;

namespace VintageStoryModManager.ViewModels.Popups
{
    public partial class ImportModpackPopupViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private readonly IGameVersionManager _gameVersionManager;

        [ObservableProperty]
        private string modpackArchivePath = string.Empty;

        [ObservableProperty]
        private string modpackName = string.Empty;

        [ObservableProperty]
        private IDictionary<string, VersionInfos> installedVersions = new Dictionary<string, VersionInfos>();

        [ObservableProperty]
        private VersionInfos? selectedVersion;

        public ImportModpackPopupViewModel(IDialogService dialogService, IGameVersionManager gameVersionManager)
        {
            _dialogService = dialogService;
            _gameVersionManager = gameVersionManager;
            _ = LoadInstalledVersion();
        }

        private async Task LoadInstalledVersion()
        {
            InstalledVersions.Clear();
            InstalledVersions = await _gameVersionManager.GetInstalledVersions();
        }

        [RelayCommand]
        private void BrowseModpackArchive()
        {
            OpenFileDialog dialog = new()
            {
                Filter = "Modpack Files (*.zip)|*.zip",
            };

            if (dialog.ShowDialog() == true)
            {
                ModpackArchivePath = dialog.FileName;
                ZipArchive modpack = ZipFile.OpenRead(ModpackArchivePath);
                ZipArchiveEntry? modpackJsonEntry = modpack.GetEntry("modpack.json");
                if (modpackJsonEntry != null)
                {
                    using (StreamReader reader = new(modpackJsonEntry.Open()))
                    {
                        string json = reader.ReadToEnd();
                        ModpackInfos? modpackInfos = JsonSerializer.Deserialize<ModpackInfos>(json);
                        if (modpackInfos != null)
                        {
                            ModpackName = modpackInfos.Name;
                            if (InstalledVersions.ContainsKey(modpackInfos.Version.Name))
                            {
                                SelectedVersion = InstalledVersions[modpackInfos.Version.Name];
                            } 
                            else
                            {
                                // TODO : Add a popup to inform the user that the version is not installed and make it possible to choose between installing it or using another version
                            }
                        }
                    }
                }
            }
        }

        [RelayCommand]
        private void Import()
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
