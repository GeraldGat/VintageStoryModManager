using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using VintageStoryModManager.Models.VintageStoryApi;
using VintageStoryModManager.Services.Interfaces;

namespace VintageStoryModManager.ViewModels.Popups
{
    public partial class ShowModPopupViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private ModInfosApi modInfos;

        public ShowModPopupViewModel(IDialogService dialogService, ModInfosApi modInfos)
        {
            _dialogService = dialogService;
            ModInfos = modInfos;
        }

        private void OpenUrl(string? url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
        }

        [RelayCommand]
        private void OpenVsModUrl() => OpenUrl($"https://mods.vintagestory.at/show/mod/{ModInfos.AssetId}");

        private bool CanOpenHomepage() => !string.IsNullOrWhiteSpace(ModInfos.HomepageUrl);
        [RelayCommand(CanExecute = nameof(CanOpenHomepage))]
        private void OpenHomepageUrl() => OpenUrl(ModInfos.HomepageUrl);

        private bool CanOpenSourceCode() => !string.IsNullOrWhiteSpace(ModInfos.SourceCodeUrl);
        [RelayCommand(CanExecute = nameof(CanOpenSourceCode))]
        private void OpenSourceCodeUrl() => OpenUrl(ModInfos.SourceCodeUrl);

        private bool CanOpenTrailerVideo() => !string.IsNullOrWhiteSpace(ModInfos.TrailerVideoUrl);
        [RelayCommand(CanExecute = nameof(CanOpenTrailerVideo))]
        private void OpenTrailerVideoUrl() => OpenUrl(ModInfos.TrailerVideoUrl);

        private bool CanOpenIssueTracker() => !string.IsNullOrWhiteSpace(ModInfos.IssueTrackerUrl);
        [RelayCommand(CanExecute = nameof(CanOpenIssueTracker))]
        private void OpenIssueTrackerUrl() => OpenUrl(ModInfos.IssueTrackerUrl);

        private bool CanOpenWiki() => !string.IsNullOrWhiteSpace(ModInfos.WikiUrl);
        [RelayCommand(CanExecute = nameof(CanOpenWiki))]
        private void OpenWikiUrl() => OpenUrl(ModInfos.WikiUrl);
    }
}
