using VintageStoryModManager.Models;
using VintageStoryModManager.Models.VintageStoryApi;

namespace VintageStoryModManager.Services.Interfaces
{
    public interface IModManager
    {
        public ModpackInfos LoadInstalledMods(ModpackInfos modpackInfo);
        public Task AddMod(ModpackInfos modpackInfos, ReleaseInfosApi releaseInfos);
        public void RemoveMod(ModpackInfos modpackInfo, ModInfos modInfo);
    }
}
