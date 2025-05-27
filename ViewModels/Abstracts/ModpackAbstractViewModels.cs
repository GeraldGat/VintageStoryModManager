using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO.Compression;
using System.IO;
using System.Windows;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services.Interfaces;
using VSModpackManager.Extensions;
using VintageStoryModManager.Views.Controls;

namespace VintageStoryModManager.ViewModels.Abstracts
{
    public abstract partial class ModpackAbstractViewModels : ObservableObject
    {
        private readonly IConfigurationService _configurationService;
        private readonly IGameVersionManager _gameVersionManager;
        private readonly IMainWindowUiService _mainWindowUiService;
        private readonly INavigationService _navigationService;

        private IDictionary<string, VersionInfos> installedVersions = new Dictionary<string, VersionInfos>();

        public ModpackAbstractViewModels(IConfigurationService configurationService, IGameVersionManager gameVersionManager, IMainWindowUiService mainWindowUiService, INavigationService navigationService)
        {
            _configurationService = configurationService;
            _gameVersionManager = gameVersionManager;
            _mainWindowUiService = mainWindowUiService;
            _navigationService = navigationService;

            LoadInstalledVersions();
        }

        private async void LoadInstalledVersions()
        {
            installedVersions = await _gameVersionManager.GetInstalledVersions();
        }

        [RelayCommand]
        protected void Start(ModpackInfos modpackInfos)
        {
            if(!installedVersions.ContainsKey(modpackInfos.Version.Name) || installedVersions[modpackInfos.Version.Name] == null)
            {
                // TODO : Add a popup to inform the user that the version is not installed and make it possible to choose between installing it or using another version
                return;
            }

            string? folderName = installedVersions[modpackInfos.Version.Name]?.FolderName;
            if (string.IsNullOrEmpty(folderName))
            {
                MessageBox.Show("An error occurred: The version couldn't be resolved correctly.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string versionFolder = Path.Combine(_configurationService.AppConfig.GameVersionsPath, folderName);

            if (!Path.Exists(versionFolder))
            {
                MessageBox.Show("An error occured: Can't find the specified version folder.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            string exeFile = Path.Combine(versionFolder, "Vintagestory.exe");
            if (!Path.Exists(exeFile))
            {
                MessageBox.Show("An error occured: Can't find the specified version executable.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            string modpackFolder = Path.Combine(_configurationService.AppConfig.ModpacksPath, modpackInfos.FolderName);
            string modpackModFolder = Path.Combine(modpackFolder, "Mods");
            if (!Path.Exists(modpackFolder))
            {
                MessageBox.Show("An error occured: Can't find the specified modpack folder.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Process.Start(exeFile, $"--dataPath \"{modpackFolder}\" --addModPath \"{modpackModFolder}\"");
        }

        [RelayCommand]
        private void Edit(ModpackInfos modpackInfos)
        {
            _mainWindowUiService.UncheckMenu();
            EditModpackPage view = _navigationService.Navigate<EditModpackPage>() as EditModpackPage;
            if (view != null)
            {
                EditModpackPageViewModel viewModel = (EditModpackPageViewModel)view.DataContext;
                viewModel.LoadInfos(modpackInfos);
            }
        }

        [RelayCommand]
        protected void Export(ModpackInfos modpackInfos)
        {
            SaveFileDialog saveFileDialog = new()
            {
                Filter = "Fichier ZIP (*.zip)|*.zip",
                FileName = $"{modpackInfos.Name}.zip"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string zipPath = saveFileDialog.FileName;

                try
                {
                    using (FileStream export = new(zipPath, FileMode.Create))
                    {
                        using (ZipArchive modpackArchive = new(export, ZipArchiveMode.Create))
                        {
                            modpackArchive.CreateEntryFromFile(Path.Combine(_configurationService.AppConfig.ModpacksPath, modpackInfos.FolderName, "modpack.json"), "modpack.json");
                            if (Directory.Exists(Path.Combine(_configurationService.AppConfig.ModpacksPath, modpackInfos.FolderName, "Mods/")))
                                modpackArchive.CreateEntriesFromDirectory(Path.Combine(_configurationService.AppConfig.ModpacksPath, modpackInfos.FolderName, "Mods/"), "Mods/");
                            if(Directory.Exists(Path.Combine(_configurationService.AppConfig.ModpacksPath, modpackInfos.FolderName, "ModConfig/")))
                                modpackArchive.CreateEntriesFromDirectory(Path.Combine(_configurationService.AppConfig.ModpacksPath, modpackInfos.FolderName, "ModConfig/"), "ModConfig/");
                        }
                    }

                    MessageBox.Show("Export successful !", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured while exporting.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        [RelayCommand]
        protected void Delete(ModpackInfos modpackInfos)
        {
            if (MessageBoxResult.No == MessageBox.Show("Are you sure you want to uninstall this modpack ?", "Confirmation", MessageBoxButton.YesNo))
            {
                return;
            }

            try
            {
                Directory.Delete(Path.Combine(_configurationService.AppConfig.ModpacksPath, modpackInfos.FolderName), true);
                _mainWindowUiService.CheckHomeMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while uninstalling the modpack.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        protected void OpenConfig(ModpackInfos modpackInfos)
        {
            string path = Path.Combine(_configurationService.AppConfig.ModpacksPath, modpackInfos.FolderName, "ModConfig");
            if (Directory.Exists(path))
            {
                Process.Start("explorer.exe", path);
            }
            else
            {
                MessageBox.Show("Directory does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        protected void OpenFolder(ModpackInfos modpackInfo)
        {
            string path = Path.Combine(_configurationService.AppConfig.ModpacksPath, modpackInfo.FolderName);
            if (Directory.Exists(path))
            {
                Process.Start("explorer.exe", path);
            }
            else
            {
                MessageBox.Show("Directory does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
