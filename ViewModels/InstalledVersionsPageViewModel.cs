using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.ViewModels
{
    public partial class InstalledVersionsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<VersionInfos> installedVersions = [];

        [RelayCommand]
        private void Add()
        {
        }

        [RelayCommand]
        private void OpenFolder(VersionInfos versionInfo)
        {
        }

        [RelayCommand]
        private void Uninstall(VersionInfos versionInfo)
        {
        }
    }
}
