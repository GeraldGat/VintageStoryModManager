using System.IO.Compression;
using VintageStoryModManager.Models;
using VintageStoryModManager.Models.VintageStoryApi;

namespace VintageStoryModManager.Services.Interfaces
{
    public interface IPopupManager
    {
        public (bool, ModpackInfos?) ShowCreateModpackPopup();
        public (bool, ModpackInfos?, ZipArchive?) ShowImportModpackPopup();
        public void ShowModPopup(ModInfosApi modInfos);
        public (bool, ReleaseInfosApi?) ShowInstallModPopup(ModInfosApi modInfos, string modpackGameVersion, string? actualModVersion = null);
        public void ShowDisplayHtmlPopup(string html);
    }
}
