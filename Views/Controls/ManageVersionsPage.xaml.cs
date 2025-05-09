using System.Windows.Controls;
using VintageStoryModManager.ViewModels;

namespace VintageStoryModManager.Views.Controls
{
    public partial class ManageVersionsPage : UserControl
    {
        public ManageVersionsPage(ManageVersionPageViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
