using VintageStoryModManager.Models;
using VintageStoryModManager.Models.VintageStoryApi;

namespace VintageStoryModManager.Services.Interfaces
{
    public interface IVintageStoryApiService
    {
        public Task<IEnumerable<VersionInfos>> GetVersionsAsync();
        public Task<IEnumerable<ModInfosApi>> GetModsAsync(
            string? search = null,
            IEnumerable<int>? tagIds = null,
            int? gameVersion = null,
            string? orderBy = "asset.created",
            string? orderDirection = "desc");
        public Task<ModInfosApi?> GetModAsync(int modId);
        public Task<ModInfosApi?> GetModAsync(string modId);
        public Task<IEnumerable<ModTag>> GetModTags();
    }
}
