using System.IO.Compression;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.Services.Interfaces
{
    public interface IModpackManager
    {
        public ModpackInfos? AddModpack(ModpackInfos modpack);
        public ModpackInfos? ImportModpack(ModpackInfos modpack, ZipArchive modpackArchive);
        public List<ModpackInfos> GetInstalledModpacks();
    }
}
