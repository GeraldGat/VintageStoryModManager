using System.Windows.Controls;
using VintageStoryModManager.ViewModels;

namespace VintageStoryModManager.Views.Controls
{
    public partial class InstalledVersionsPage : UserControl
    {
        public InstalledVersionsPage(InstalledVersionsPageViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
