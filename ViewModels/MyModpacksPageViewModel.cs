using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services.Interfaces;

namespace VintageStoryModManager.ViewModels
{
    public partial class MyModpacksPageViewModel : ObservableObject
    {
        private readonly IModpackManager _modpackManager;
        private readonly IPopupManager _popupManager;

        [ObservableProperty]
        private ObservableCollection<ModpackInfos> modpacks = [];

        public MyModpacksPageViewModel(IModpackManager modpackManager, IPopupManager popupManager)
        {
            _modpackManager = modpackManager;
            _popupManager = popupManager;

            LoadModpacks();
        }

        private void LoadModpacks()
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
    }
}
