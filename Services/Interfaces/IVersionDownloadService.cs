using VintageStoryModManager.Models;

namespace VintageStoryModManager.Services.Interfaces
{
    public interface IVersionDownloadService
    {
        public Task<IEnumerable<VersionInfos>> CheckAvailableDownloadsAsync(IEnumerable<VersionInfos> versions);
        public Task<string> DownloadVersion(VersionInfos version, string filePath);
    }
}
