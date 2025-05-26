using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http;
using VintageStoryModManager.Converters;

namespace VintageStoryModManager.Models.VintageStoryApi
{
    public class ApiResponseItem
    {
        [JsonExtensionData]
        public Dictionary<string, JsonElement>? Data { get; set; }

        public static async Task<T?> GetItem<T>(HttpResponseMessage response) where T : class
        {
            var jsonSerialiserOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = {
                    new JsonDateTimeConverter()
                }
            };
            try
            {
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponseItem>(jsonResponse, jsonSerialiserOptions);
                if (apiResponse == null || apiResponse.Data == null || !apiResponse.Data.Any())
                    return null;
                var itemJson = apiResponse.Data.First().Value;
                var item = JsonSerializer.Deserialize<T>(itemJson.ToString() ?? string.Empty, jsonSerialiserOptions);
                return item;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
