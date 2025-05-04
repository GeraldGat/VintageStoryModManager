using System.Windows;
using VintageStoryModManager.ViewModels;

namespace VintageStoryModManager.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
