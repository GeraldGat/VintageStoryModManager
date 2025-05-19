using System.Collections.Specialized;
using System.Net.Http;
using System.Text.Json;
using System.Web;
using VintageStoryModManager.Constants;
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
            PropertyNameCaseInsensitive = true
        };

        public async Task<IEnumerable<VersionInfos>> GetVersionsAsync()
        {
            var response = await _httpClient.GetAsync("gameversions");
            try
            {
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                GameVersionsApiResponse? gameVersionApiResponse = JsonSerializer.Deserialize<GameVersionsApiResponse>(jsonResponse, _jsonSerializerOptions);
                return gameVersionApiResponse?.GameVersions ?? [];
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
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                ModsInfosApiResponse? modsInfosApiResponse = JsonSerializer.Deserialize<ModsInfosApiResponse>(jsonResponse, _jsonSerializerOptions);
                return modsInfosApiResponse?.Mods ?? [];
            }
            catch (Exception ex)
            {
                return [];
            }
        }

        public async Task<ModInfosApi?> GetModAsync(int modId)
        {
            var response = await _httpClient.GetAsync($"mod/{modId}");
            try
            {
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                ModInfosApi? modsInfosApi = JsonSerializer.Deserialize<ModInfosApi>(jsonResponse, _jsonSerializerOptions);
                return modsInfosApi;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
