using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http;

namespace VintageStoryModManager.Models.VintageStoryApi
{
    public class ApiResponse
    {
        public required string StatusCode { get; set; }
        [JsonExtensionData]
        public Dictionary<string, JsonElement>? Data { get; set; }

        public static async Task<(string statusCode, IEnumerable<T> items)> GetItems<T>(HttpResponseMessage response)
        {
            var jsonSerialiserOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            try
            {
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonResponse, jsonSerialiserOptions);
                if (apiResponse == null || apiResponse.Data == null || !apiResponse.Data.Any())
                    return (string.Empty, Enumerable.Empty<T>());
                var items = apiResponse.Data.First().Value;
                var itemsList = JsonSerializer.Deserialize<IEnumerable<T>>(items.ToString() ?? string.Empty, jsonSerialiserOptions);
                return (apiResponse.StatusCode, itemsList ?? []);
            }
            catch (Exception e)
            {
                return (string.Empty, Enumerable.Empty<T>());
            }
        }
    }
}
