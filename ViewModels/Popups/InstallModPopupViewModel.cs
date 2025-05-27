using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VintageStoryModManager.Constants;
using VintageStoryModManager.Models.VintageStoryApi;
using VintageStoryModManager.Services.Interfaces;

namespace VintageStoryModManager.ViewModels.Popups
{
    public partial class InstallModPopupViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private readonly IPopupManager _popupManager;

        public ReleaseInfosApi? SelectedRelease { get; set; }

        [ObservableProperty]
        private ObservableCollection<ReleaseInfosApi> releases;

        [ObservableProperty]
        private string? installedVersion;

        public InstallModPopupViewModel(IDialogService dialogService, IPopupManager popupManager, ModInfosApi modInfos, string gameVersion, string? actualModVersion = null)
        {
            _dialogService = dialogService;
            _popupManager = popupManager;

            modInfos = ModVersionCompatibility.GetReleasesCompatibility(modInfos, gameVersion);
            Releases = [..modInfos.Releases ?? []];

            installedVersion = actualModVersion;
        }

        [RelayCommand]
        private void ShowChangelog(ReleaseInfosApi releaseInfosApi)
        {
            if (string.IsNullOrEmpty(releaseInfosApi.Changelog))
                return;

            _popupManager.ShowDisplayHtmlPopup($"<h1>{releaseInfosApi.ModVersion}</h1>{releaseInfosApi.Changelog}");
        }

        [RelayCommand]
        private void Download(ReleaseInfosApi releaseInfosApi)
        {
            SelectedRelease = releaseInfosApi;
            _dialogService.CloseDialog(true);
        }
    }
}
