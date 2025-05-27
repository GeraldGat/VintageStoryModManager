using System.IO.Compression;
using System.Net.Http;
using VintageStoryModManager.Models;
using VintageStoryModManager.Models.VintageStoryApi;
using VintageStoryModManager.Services.Interfaces;
using VintageStoryModManager.ViewModels.Popups;
using VintageStoryModManager.Views.Popups;
using static System.Net.Mime.MediaTypeNames;

namespace VintageStoryModManager.Services
{
    internal class PopupManager : IPopupManager
    {
        private readonly IGameVersionManager _gameVersionManager;
        private readonly IMainWindowUiService _mainWindowUiService;
        private readonly IHtmlThemeManager _htmlThemeManager;

        public PopupManager(IGameVersionManager gameVersionManager, IMainWindowUiService mainWindowUiService, IHtmlThemeManager htmlThemeManager)
        {
            _gameVersionManager = gameVersionManager;
            _mainWindowUiService = mainWindowUiService;
            _htmlThemeManager = htmlThemeManager;
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

        public void ShowModPopup(ModInfosApi modInfos)
        {
            _mainWindowUiService.ShowOverlay();
            var popup = new ShowModPopup();
            var dialogService = new DialogService(popup);
            var viewModel = new ShowModPopupViewModel(dialogService, modInfos);
            popup.DataContext = viewModel;
            popup.Owner = _mainWindowUiService.MainWindow;

            if (modInfos.Text != null)
                popup.LoadHtmlRichText(_htmlThemeManager.WrapInTemplate(modInfos.Text));

            bool? result = popup.ShowDialog();
            _mainWindowUiService.HideOverlay();
        }

        public (bool, ReleaseInfosApi?) ShowInstallModPopup(ModInfosApi modInfos, string modpackGameVersion, string? actualModVersion = null)
        {
            _mainWindowUiService.ShowOverlay();
            var popup = new InstallModPopup();
            var dialogService = new DialogService(popup);
            var viewModel = new InstallModPopupViewModel(dialogService, this, modInfos, modpackGameVersion, actualModVersion);
            popup.DataContext = viewModel;
            popup.Owner = _mainWindowUiService.MainWindow;

            bool? result = popup.ShowDialog();
            _mainWindowUiService.HideOverlay();

            if (result == true && viewModel.SelectedRelease != null)
            {
                return (true, viewModel.SelectedRelease);
            }

            return (false, null);
        }

        public void ShowDisplayHtmlPopup(string html)
        {
            _mainWindowUiService.ShowOverlay();
            var popup = new DisplayHtmlPopup();
            var dialogService = new DialogService(popup);
            popup.Owner = _mainWindowUiService.MainWindow;

            popup.LoadHtmlRichText(_htmlThemeManager.WrapInTemplate(html));

            bool? result = popup.ShowDialog();
            _mainWindowUiService.HideOverlay();
        }
    }
}
