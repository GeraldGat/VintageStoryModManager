using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VintageStoryModManager.Services.Interfaces;
using VintageStoryModManager.ViewModels;

namespace VintageStoryModManager.Views
{
    public partial class MainWindow : Window
    {
        public RadioButton HomeMenu => MenuMyModpack;

        public MainWindow(INavigationService navigationService, MainWindowViewModel viewModel)
        {
            InitializeComponent();

            navigationService.SetContentControl(MainContent);

            DataContext = viewModel;

            Loaded += (s, e) =>
            {
                HomeMenu.IsChecked = true;
            };
        }
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
