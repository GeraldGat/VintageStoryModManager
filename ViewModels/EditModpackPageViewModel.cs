using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services.Interfaces;
using VintageStoryModManager.ViewModels.Abstracts;

namespace VintageStoryModManager.ViewModels
{
    public partial class EditModpackPageViewModel : ModpackAbstractViewModels
    {
        private readonly IModManager _modManager;

        [ObservableProperty]
        private ModpackInfos? modpackInfos;

        [ObservableProperty]
        private ObservableCollection<ModInfos> mods = [];

        [ObservableProperty]
        private ObservableCollection<ModInfos> availableMods = [];

        [ObservableProperty]
        private string searchText = String.Empty;

        [ObservableProperty]
        private string searchAvailableText = String.Empty;

        public EditModpackPageViewModel(
            IConfigurationService configurationService,
            IGameVersionManager gameVersionManager,
            IMainWindowUiService mainWindowUiService,
            IModManager modManager,
            INavigationService navigationService
        ) : base(configurationService, gameVersionManager, mainWindowUiService, navigationService)
        {
            _modManager = modManager;
        }

        public void LoadInfos(ModpackInfos modpackInfos)
        {
            ModpackInfos = modpackInfos;
            _modManager.LoadInstalledMods(modpackInfos);
            if(ModpackInfos != null && ModpackInfos.Mods != null)
                Mods = [..ModpackInfos.Mods.Values.ToList()];
        }
    }
}
