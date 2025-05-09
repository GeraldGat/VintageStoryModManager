using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services.Interfaces;

namespace VintageStoryModManager.Services
{
    class VersionDownloadService(HttpClient httpClient) : IVersionDownloadService
    {
        private readonly HttpClient _httpClient = httpClient;
        private JsonSerializerOptions _jsonSerializerOptions = new();

        public async Task<IEnumerable<VersionInfos>> CheckAvailableDownloadsAsync(IEnumerable<VersionInfos> versions)
        {
            var result = new List<VersionInfos>();

            foreach (var version in versions)
            {
                string versionStr = version.Name.TrimStart('v');
                string type = versionStr.Contains('-') ? "unstable" : "stable";

                var urls = new[] {
                    $"https://cdn.vintagestory.at/gamefiles/{type}/vs_install_win-x64_{versionStr}.exe",
                    $"https://cdn.vintagestory.at/gamefiles/{type}/vs_install_{versionStr}.exe"
                };

                foreach (var url in urls)
                {
                    var req = new HttpRequestMessage(HttpMethod.Head, url);
                    try
                    {
                        var response = await _httpClient.SendAsync(req);
                        if (response.IsSuccessStatusCode)
                        {
                            version.DownloadUrl = url;
                            result.Add(version);
                            break;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return result;
        }

        public async Task<string> DownloadVersion(VersionInfos version, string filepath)
        {
            using(var downloadStream = await _httpClient.GetStreamAsync(version.DownloadUrl))
            {
                using (var fileStream = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await downloadStream.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                    fileStream.Close();
                }
            }

            return filepath;
        }
    }
}
