using System.Windows;
using VintageStoryModManager.Services.Interfaces;
using VintageStoryModManager.ViewModels;
using VintageStoryModManager.Views;

namespace VintageStoryModManager.Services
{
    internal class MainWindowUiService : IMainWindowUiService
    {
        public Window MainWindow
        {
            get => _mainWindow;
            set => _mainWindow = (MainWindow)value;
        }

        private MainWindow _mainWindow = null!;

        public void ShowOverlay() => _mainWindow.Overlay.Visibility = Visibility.Visible;

        public void HideOverlay() => _mainWindow.Overlay.Visibility = Visibility.Collapsed;

        public void UncheckMenu()
        {
            var mainWindowViewModel = _mainWindow.DataContext as MainWindowViewModel ?? throw new InvalidOperationException("DataContext is not of type MainWindowViewModel");
            mainWindowViewModel.SelectedMenu = null;
        }

        public void CheckHomeMenu()
        {
            _mainWindow.HomeMenu.IsChecked = true;
        }
    }
}
