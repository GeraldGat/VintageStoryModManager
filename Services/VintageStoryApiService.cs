using System.Net.Http;
using System.Text.Json;
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
    }
}
