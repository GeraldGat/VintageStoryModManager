using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using VintageStoryModManager.Services.Interfaces;

namespace VintageStoryModManager.Services
{
    internal class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private ContentControl? _contentControl;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void SetContentControl(ContentControl contentControl)
        {
            _contentControl = contentControl;
        }

        public void Navigate<TView>() where TView : UserControl
        {
            if (_contentControl == null)
                throw new InvalidOperationException("ContentControl not set in NavigationService.");

            var view = _serviceProvider.GetRequiredService<TView>();
            _contentControl.Content = view;
        }
    }
}
