using System.Windows;
using VintageStoryModManager.Services.Interfaces;

namespace VintageStoryModManager.Services
{
    internal class DialogService : IDialogService
    {
        private readonly Window _window;

        public DialogService(Window window)
        {
            _window = window;
        }

        public void CloseDialog(bool? dialogResult)
        {
            _window.DialogResult = dialogResult;
            _window.Close();
        }
    }
}
