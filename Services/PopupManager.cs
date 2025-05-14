using System.IO.Compression;
using System.Windows;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services.Interfaces;
using VintageStoryModManager.ViewModels.Popups;
using VintageStoryModManager.Views.Popups;

namespace VintageStoryModManager.Services
{
    internal class PopupManager : IPopupManager
    {
        private readonly IGameVersionManager _gameVersionManager;

        public PopupManager(IGameVersionManager gameVersionManager)
        {
            _gameVersionManager = gameVersionManager;
        }

        public (bool, ModpackInfos?) ShowCreateModpackPopup()
        {
            var popup = new CreateModpackPopup();
            var dialogService = new DialogService(popup);
            var viewModel = new CreateModpackPopupViewModel(dialogService, _gameVersionManager);
            popup.DataContext = viewModel;
            popup.Owner = Application.Current.MainWindow;

            bool? result = popup.ShowDialog();

            if (result == true && !string.IsNullOrWhiteSpace(viewModel.ModpackName) && viewModel.SelectedVersion != null)
            {
                var modpackInfo = new ModpackInfos()
                {
                    Name = viewModel.ModpackName,
                    Version = viewModel.SelectedVersion
                };
                return (true, modpackInfo);
            }
            return (false, null);
        }

        public (bool, ModpackInfos?, ZipArchive?) ShowImportModpackPopup()
        {
            var popup = new ImportModpackPopup();
            var dialogService = new DialogService(popup);
            var viewModel = new ImportModpackPopupViewModel(dialogService, _gameVersionManager);
            popup.DataContext = viewModel;
            popup.Owner = Application.Current.MainWindow;
            bool? result = popup.ShowDialog();
            if (result == true && !string.IsNullOrWhiteSpace(viewModel.ModpackName) && viewModel.SelectedVersion != null && !string.IsNullOrWhiteSpace(viewModel.ModpackArchivePath))
            {
                var modpackInfo = new ModpackInfos()
                {
                    Name = viewModel.ModpackName,
                    Version = viewModel.SelectedVersion
                };
                var modpackArchive = ZipFile.OpenRead(viewModel.ModpackArchivePath);
                return (true, modpackInfo, modpackArchive);
            }
            return (false, null, null);
        }
    }
}
