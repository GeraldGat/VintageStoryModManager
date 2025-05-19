using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using VintageStoryModManager.Extensions;
using VintageStoryModManager.Models;
using VintageStoryModManager.Models.VintageStoryApi;
using VintageStoryModManager.Services.Interfaces;
using VintageStoryModManager.ViewModels.Abstracts;
using static VintageStoryModManager.Constants.ModApiFilters;

namespace VintageStoryModManager.ViewModels
{
    public partial class EditModpackPageViewModel : ModpackAbstractViewModels
    {
        private readonly IModManager _modManager;
        private readonly IVintageStoryApiService _vintageStoryApiService;

        [ObservableProperty]
        private ModpackInfos? modpackInfos;

        [ObservableProperty]
        private ObservableCollection<ModInfos> mods = [];

        [ObservableProperty]
        private ObservableCollection<ModInfosApi> availableMods = [];

        [ObservableProperty]
        private string searchText = String.Empty;

        [ObservableProperty]
        private string textFilter = String.Empty;

        [ObservableProperty]
        private VersionInfos? gameVersionFilter;

        [ObservableProperty]
        private KeyValuePair<string, string?>? orderByFilter = new KeyValuePair<string, string?>("Created", OrderBy.Created);

        [ObservableProperty]
        private KeyValuePair<string, string?>? orderDirectionFilter = new KeyValuePair<string, string?>("Descending", OrderDirection.Descending);

        [ObservableProperty]
        private IReadOnlyCollection<VersionInfos> gameVersions = [];

        [ObservableProperty]
        private List<KeyValuePair<string, string?>> orderByList = GetConstantItems(typeof(OrderBy));

        [ObservableProperty]
        private List<KeyValuePair<string, string?>> orderDirectionList = GetConstantItems(typeof(OrderDirection));

        private Task _loadGameVersionTask;

        public EditModpackPageViewModel(
            IConfigurationService configurationService,
            IGameVersionManager gameVersionManager,
            IMainWindowUiService mainWindowUiService,
            IModManager modManager,
            INavigationService navigationService,
            IVintageStoryApiService vintageStoryApiService
        ) : base(configurationService, gameVersionManager, mainWindowUiService, navigationService)
        {
            _modManager = modManager;
            _vintageStoryApiService = vintageStoryApiService;

            _loadGameVersionTask = LoadGameVersions();
        }

        public async void LoadInfos(ModpackInfos modpackInfos)
        {
            ModpackInfos = modpackInfos;
            _modManager.LoadInstalledMods(modpackInfos);
            if(ModpackInfos != null && ModpackInfos.Mods != null)
                Mods = [..ModpackInfos.Mods.Values.ToList()];

            await _loadGameVersionTask;
            GameVersionFilter = GameVersions.FirstOrDefault(x => x.TagId == modpackInfos.Version.TagId);

            _ = LoadAvailableMods();
        }

        public async Task LoadAvailableMods()
        {
            AvailableMods = [.. (await _vintageStoryApiService.GetModsAsync(TextFilter, null, GameVersionFilter?.TagId, OrderByFilter?.Value, OrderDirectionFilter?.Value)).Take(20)];
        }

        public async Task LoadGameVersions()
        {
            GameVersions = [new VersionInfos { TagId=null, Name=""},.. (await _vintageStoryApiService.GetVersionsAsync())];
        }

        [RelayCommand]
        public void DeleteMod(ModInfos modInfos)
        {
            if (MessageBoxResult.No == MessageBox.Show("Are you sure you want to remove this mod ?", "Confirmation", MessageBoxButton.YesNo) || ModpackInfos == null)
            {
                return;
            }

            _modManager.RemoveMod(ModpackInfos, modInfos);
            if (ModpackInfos != null && ModpackInfos.Mods != null)
                Mods = [.. ModpackInfos.Mods.Values.ToList()];
        }

        partial void OnSearchTextChanged(string value)
        {
            if (ModpackInfos?.Mods != null)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Mods = [.. ModpackInfos.Mods.Values.Where(mod => mod.Name.Contains(value, StringComparison.OrdinalIgnoreCase) || mod.Description?.Contains(value, StringComparison.OrdinalIgnoreCase) == true)];
                }
                else
                {
                    Mods = [.. ModpackInfos.Mods.Values.ToList()];
                }
            }
        }

        partial void OnTextFilterChanged(string value)
        {
            _ = LoadAvailableMods();
        }

        partial void OnGameVersionFilterChanged(VersionInfos? value)
        {
            _ = LoadAvailableMods();
        }

        partial void OnOrderByFilterChanged(KeyValuePair<string, string?>? value)
        {
            _ = LoadAvailableMods();
        }

        partial void OnOrderDirectionFilterChanged(KeyValuePair<string, string?>? value)
        {
            _ = LoadAvailableMods();
        }
    }
}
