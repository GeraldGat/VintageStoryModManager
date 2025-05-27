using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO.Compression;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services.Interfaces;
using VintageStoryModManager.ViewModels.Abstracts;

namespace VintageStoryModManager.ViewModels
{
    public partial class MyModpacksPageViewModel : ModpackAbstractViewModels
    {
        private readonly IModpackManager _modpackManager;
        private readonly IPopupManager _popupManager;

        [ObservableProperty]
        private ObservableCollection<ModpackInfos> modpacks = [];

        public MyModpacksPageViewModel(
            IConfigurationService configurationService,
            IGameVersionManager gameVersionManager,
            IMainWindowUiService mainWindowUiService,
            IModpackManager modpackManager,
            INavigationService navigationService,
            IPopupManager popupManager
        ) : base(configurationService, gameVersionManager, mainWindowUiService, navigationService)
        {
            _modpackManager = modpackManager;
            _popupManager = popupManager;

            LoadModpacks();
        }

        protected void LoadModpacks()
        {
            Modpacks = [.. _modpackManager.GetInstalledModpacks()];
        }

        [RelayCommand]
        private void Create()
        {
            (bool isValid, ModpackInfos? modpack) = _popupManager.ShowCreateModpackPopup();

            if (isValid && modpack != null)
            {
                _modpackManager.AddModpack(modpack);
                Modpacks.Add(modpack);
            }
        }

        [RelayCommand]
        private void Import()
        {
            (bool isValid, ModpackInfos? modpack, ZipArchive? modpackArchive) = _popupManager.ShowImportModpackPopup();

            if (isValid && modpack != null && modpackArchive != null)
            {
                _modpackManager.ImportModpack(modpack, modpackArchive);
                Modpacks.Add(modpack);
            }
        }
    }
}
