using System.Windows.Controls;
using VintageStoryModManager.ViewModels;

namespace VintageStoryModManager.Views.Controls
{
    public partial class SettingsPage : UserControl
    {
        public SettingsPage(SettingsPageViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
