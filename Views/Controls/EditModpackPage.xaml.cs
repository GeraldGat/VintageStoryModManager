using System.Windows.Controls;
using VintageStoryModManager.ViewModels;

namespace VintageStoryModManager.Views.Controls
{
    /// <summary>
    /// Interaction logic for EditModpackPage.xaml
    /// </summary>
    public partial class EditModpackPage : UserControl
    {
        public EditModpackPage(EditModpackPageViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
