using System.Windows.Controls;
using VintageStoryModManager.ViewModels;

namespace VintageStoryModManager.Views.Controls
{
    public partial class MyModpacksPage : UserControl
    {
        public MyModpacksPage(MyModpacksPageViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
