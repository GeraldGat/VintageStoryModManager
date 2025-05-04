using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VintageStoryModManager.Services.Interfaces
{
    public interface INavigationService
    {
        void SetContentControl(ContentControl contentControl);
        void Navigate<TView>() where TView : UserControl;
    }
}
