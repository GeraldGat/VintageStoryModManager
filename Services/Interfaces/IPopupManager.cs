using VintageStoryModManager.Models;

namespace VintageStoryModManager.Services.Interfaces
{
    public interface IPopupManager
    {
        public (bool, ModpackInfos?) ShowCreateModpackPopup();
    }
}
