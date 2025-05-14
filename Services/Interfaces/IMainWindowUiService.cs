using System.Windows;

namespace VintageStoryModManager.Services.Interfaces
{
    public interface IMainWindowUiService
    {
        public abstract Window MainWindow { get; set; }
        public void ShowOverlay();
        public void HideOverlay();
        public void UncheckMenu();
        public void CheckHomeMenu();
    }
}
