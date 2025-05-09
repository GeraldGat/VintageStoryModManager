using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using VintageStoryModManager.Services.Interfaces;
using VintageStoryModManager.Views.Controls;

namespace VintageStoryModManager.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private string? selectedMenu;

        public MainWindowViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand]
        partial void OnSelectedMenuChanged(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;

            switch (value)
            {
                case "Browse":
                    _navigationService.Navigate<BrowseModpacksPage>();
                    break;
                case "Installed":
                    _navigationService.Navigate<ManageVersionsPage>();
                    break;
                case "Settings":
                    _navigationService.Navigate<SettingsPage>();
                    break;
                default:
                    _navigationService.Navigate<MyModpacksPage>();
                    break;
            }
        }
    }
}
