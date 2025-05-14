using System.IO.Compression;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.Services.Interfaces
{
    public interface IModpackManager
    {
        public ModpackInfos? AddModpack(ModpackInfos modpackInfos);
        public ModpackInfos? ImportModpack(ModpackInfos modpackInfos, ZipArchive modpackArchive);
        public List<ModpackInfos> GetInstalledModpacks();
    }
}
