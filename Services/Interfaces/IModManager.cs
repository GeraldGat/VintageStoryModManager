using VintageStoryModManager.Models;

namespace VintageStoryModManager.Services.Interfaces
{
    public interface IModManager
    {
        public ModpackInfos LoadInstalledMods(ModpackInfos modpackInfo);
        public void AddMod(ModpackInfos modpackInfo, ModInfos modInfo);
        public void RemoveMod(ModpackInfos modpackInfo, ModInfos modInfo);
    }
}
