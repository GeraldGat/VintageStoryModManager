using System.IO.Compression;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.Services.Interfaces
{
    public interface IPopupManager
    {
        public (bool, ModpackInfos?) ShowCreateModpackPopup();
        public (bool, ModpackInfos?, ZipArchive?) ShowImportModpackPopup();
    }
}
