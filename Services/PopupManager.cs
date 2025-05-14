using System.IO.Compression;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services.Interfaces;
using VintageStoryModManager.ViewModels.Popups;
using VintageStoryModManager.Views.Popups;

namespace VintageStoryModManager.Services
{
    internal class PopupManager : IPopupManager
    {
        private readonly IGameVersionManager _gameVersionManager;
        private readonly IMainWindowUiService _mainWindowUiService;

        public PopupManager(IGameVersionManager gameVersionManager, IMainWindowUiService mainWindowUiService)
        {
            _gameVersionManager = gameVersionManager;
            _mainWindowUiService = mainWindowUiService;
        }

        public (bool, ModpackInfos?) ShowCreateModpackPopup()
        {
            _mainWindowUiService.ShowOverlay();
            var popup = new CreateModpackPopup();
            var dialogService = new DialogService(popup);
            var viewModel = new CreateModpackPopupViewModel(dialogService, _gameVersionManager);
            popup.DataContext = viewModel;
            popup.Owner = _mainWindowUiService.MainWindow;

            bool? result = popup.ShowDialog();
            _mainWindowUiService.HideOverlay();

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
            _mainWindowUiService.ShowOverlay();
            var popup = new ImportModpackPopup();
            var dialogService = new DialogService(popup);
            var viewModel = new ImportModpackPopupViewModel(dialogService, _gameVersionManager);
            popup.DataContext = viewModel;
            popup.Owner = _mainWindowUiService.MainWindow;

            bool? result = popup.ShowDialog();
            _mainWindowUiService.HideOverlay();

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
