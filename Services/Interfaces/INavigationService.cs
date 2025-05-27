using System.Windows.Controls;

namespace VintageStoryModManager.Services.Interfaces
{
    public interface INavigationService
    {
        void SetContentControl(ContentControl contentControl);
        TView Navigate<TView>() where TView : UserControl;
    }
}
