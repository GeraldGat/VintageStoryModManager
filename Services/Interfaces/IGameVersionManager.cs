using VintageStoryModManager.Models;

namespace VintageStoryModManager.Services.Interfaces
{
    public interface IGameVersionManager
    {
        public Task<IDictionary<string, VersionInfos>> GetAvailableAndInstalledVersionsAndDownloadUrlAsync(bool forceReload);
        public Task<IDictionary<string, VersionInfos>> GetInstalledVersions();
        public Task<VersionInfos?> AddGameVersion(VersionInfos version);
        public Task<VersionInfos> RemoveGameVersion(VersionInfos version);
    }
}
