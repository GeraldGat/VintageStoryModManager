using System.Net.Http;
using System.Text.Json;
using System.Web;
using VintageStoryModManager.Constants;
using VintageStoryModManager.Converters;
using VintageStoryModManager.Models;
using VintageStoryModManager.Models.VintageStoryApi;
using VintageStoryModManager.Services.Interfaces;

namespace VintageStoryModManager.Services
{
    class VintageStoryApiService(HttpClient httpClient) : IVintageStoryApiService
    {
        private readonly HttpClient _httpClient = httpClient;
        private JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = {
                new JsonDateTimeConverter()
            }
        };

        public async Task<IEnumerable<VersionInfos>> GetVersionsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("gameversions");
                var (status, gamesVersions) = await ApiResponseList.GetItems<VersionInfos>(response);
                return gamesVersions;
            } 
            catch (Exception e)
            {
                return [];
            }
        }

        public async Task<IEnumerable<ModInfosApi>> GetModsAsync(
            string? search = null,
            IEnumerable<int>? tagIds = null,
            int? gameVersion = null,
            string? orderBy = ModApiFilters.OrderBy.Created,
            string? orderDirection = ModApiFilters.OrderDirection.Descending)
        {
            try
            {
                var query = HttpUtility.ParseQueryString(string.Empty);
                if (!string.IsNullOrWhiteSpace(search))
                    query.Add("text", search);

                if (gameVersion != null)
                    query.Add("gv", gameVersion.ToString());

                if (!string.IsNullOrWhiteSpace(orderBy))
                    query.Add("orderby", orderBy);

                if (!string.IsNullOrWhiteSpace(orderDirection))
                    query.Add("orderdirection", orderDirection);

                if (tagIds != null)
                {
                    foreach (var tagId in tagIds)
                    {
                        query.Add("tagids[]", tagId.ToString());
                    }
                }

                var uriBuilder = new UriBuilder(_httpClient.BaseAddress + "mods")
                {
                    Query = query.ToString()
                };

                var response = await _httpClient.GetAsync(uriBuilder.Uri);
                var (status, modsInfos) = await ApiResponseList.GetItems<ModInfosApi>(response);
                return modsInfos;
            }
            catch (Exception ex)
            {
                return [];
            }
        }

        public async Task<ModInfosApi?> GetModAsync(int modId)
        {
            return await GetModAsyncFromEndpoint($"mod/{modId}");
        }

        public async Task<ModInfosApi?> GetModAsync(string modId)
        {
            return await GetModAsyncFromEndpoint($"mod/{modId}");
        }

        private async Task<ModInfosApi?> GetModAsyncFromEndpoint(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                var modInfosApi = await ApiResponseItem.GetItem<ModInfosApi>(response);
                return modInfosApi;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ModTag>> GetModTags()
        {
            try
            {
                var response = await _httpClient.GetAsync("tags");
                var (status, tags) = await ApiResponseList.GetItems<ModTag>(response);
                return tags;
            }
            catch (Exception e)
            {
                return [];
            }
        }
    }
}
