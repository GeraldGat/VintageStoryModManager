using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageStoryModManager.Services.Interfaces
{
    public interface IThemeManager
    {
        public void ApplyTheme(string theme);
        public static string GetAppColorThemeFromSystem() => throw new NotImplementedException();
    }
}
