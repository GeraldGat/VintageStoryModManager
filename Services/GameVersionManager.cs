using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services.Interfaces;

namespace VintageStoryModManager.Services
{
    class GameVersionManager(IVintageStoryApiService vintageStoryApiService, IVersionDownloadService versionDownloadService, IConfigurationService configurationService) : IGameVersionManager
    {
        private readonly IVintageStoryApiService _vintageStoryApiService = vintageStoryApiService;
        private readonly IVersionDownloadService _versionDownloadService = versionDownloadService;
        private readonly IConfigurationService _configurationService = configurationService;
        private JsonSerializerOptions _jsonSerializerOptions = new();

        public async Task<IDictionary<string, VersionInfos>> GetAvailableAndInstalledVersionsAndDownloadUrlAsync(bool forceReload = false)
        {
            IDictionary<string, VersionInfos>? versions;

            if (!forceReload && File.Exists(App.AppAvailableVersionsPath))
            {
                var json = await File.ReadAllTextAsync(App.AppAvailableVersionsPath);
                versions = JsonSerializer.Deserialize<IDictionary<string, VersionInfos>>(json);
            }
            else
            {
                var versionsList = await _vintageStoryApiService.GetVersionsAsync();
                versionsList = await _versionDownloadService.CheckAvailableDownloadsAsync(versionsList);

                versionsList = versionsList.Where(v => !String.IsNullOrWhiteSpace(v.DownloadUrl));

                versions = FormatVersionList(versionsList);

                var json = JsonSerializer.Serialize(versions, _jsonSerializerOptions);
                File.WriteAllText(App.AppAvailableVersionsPath, json);
            }

            if (versions == null)
            {
                return new Dictionary<string, VersionInfos>();
            }

            foreach (var directory in Directory.GetDirectories(_configurationService.AppConfig.GameVersionsPath, "*", SearchOption.TopDirectoryOnly))
            {
                var assetsDir = Path.Combine(directory, "assets");

                if (!Directory.Exists(assetsDir))
                    continue;

                var versionFile = Directory.GetFiles(assetsDir, "version-*.txt", SearchOption.TopDirectoryOnly).FirstOrDefault();

                if (versionFile is not null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(versionFile);
                    if (fileName.StartsWith("version-", StringComparison.OrdinalIgnoreCase))
                    {
                        var version = "v" + fileName.Substring("version-".Length);

                        if (versions.ContainsKey(version))
                        {
                            versions[version].FolderName = directory;
                        }
                    }
                }
            }

            return versions;
        }

        private IDictionary<string, VersionInfos> FormatVersionList(IEnumerable<VersionInfos> versions)
        {
            Dictionary<string, VersionInfos> formatedVersions = [];
            foreach (var version in versions)
            {
                formatedVersions[version.Name] = version;
            }
            return formatedVersions;
        }

        public async Task<VersionInfos?> AddGameVersion(VersionInfos version)
        {
            string tempDir = Path.Combine(_configurationService.AppConfig.GameVersionsPath, $"tmp_{Guid.NewGuid()}");
            string folderName = Guid.NewGuid().ToString();
            string versionDir = Path.Combine(_configurationService.AppConfig.GameVersionsPath, folderName);

            try
            {
                Directory.CreateDirectory(tempDir);
                var installerPath = Path.Combine(tempDir, "installer.exe");
                var downloadPath = await _versionDownloadService.DownloadVersion(version, installerPath);

                Process process = new()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Innoextract", "innoextract.exe"),
                        Arguments = $"--extract --silent --no-warn-unused --output-dir \"{tempDir}\" \"{installerPath}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                await Task.Run(() =>
                {
                    process.Start();
                    process.WaitForExit();
                });

                string extractedAppPath = Path.Combine(tempDir, "app");
                if (!Directory.Exists(extractedAppPath))
                {
                    MessageBox.Show("Extraction failed. The 'app' directory was not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                Directory.Move(extractedAppPath, versionDir);
                Directory.Delete(tempDir, true);

                string exePath = Path.Combine(versionDir, "Vintagestory.exe");
                if (!File.Exists(exePath))
                {
                    MessageBox.Show("Installation completed, but Vintagestory.exe was not found in the target folder.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                version.FolderName = versionDir;

                return version;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while downloading or installing the version.", "Installation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }

        public async Task<VersionInfos> RemoveGameVersion(VersionInfos version)
        {
            if (version.FolderName != null && MessageBoxResult.Yes == MessageBox.Show("Are you sure you want to uninstall this version ?", "Confirmation", MessageBoxButton.YesNo))
            {
                try
                {
                    await Task.Run(() => Directory.Delete(Path.Combine(_configurationService.AppConfig.GameVersionsPath, version.FolderName), true));
                    version.FolderName = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while uninstalling the game version.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return version;
        }
    }
}
