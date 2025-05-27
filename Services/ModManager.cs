using System.IO.Compression;
using System.IO;
using System.Text.Json;
using System.Windows.Media.Imaging;
using System.Windows;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services.Interfaces;
using VintageStoryModManager.Models.VintageStoryApi;
using System.Net.Http;

namespace VintageStoryModManager.Services
{
    internal class ModManager : IModManager
    {
        private readonly IConfigurationService _configurationService;

        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ModManager(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public ModpackInfos LoadInstalledMods(ModpackInfos modpackInfos)
        {
            Dictionary<string, ModInfos> installedMods = [];

            string path = Path.Combine(_configurationService.AppConfig.ModpacksPath, modpackInfos.FolderName, "Mods");
            if (!Directory.Exists(path))
            {
                modpackInfos.Mods = installedMods;
                return modpackInfos;
            }

            var defaultImage = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/mod-default.png"));
            foreach (var file in Directory.GetFiles(path, "*.zip"))
            {
                using (ZipArchive modArchive = ZipFile.OpenRead(file))
                {
                    var modInfoEntry = modArchive.GetEntry("modinfo.json");
                    if (modInfoEntry == null)
                        continue;
                    using (StreamReader reader = new(modInfoEntry.Open()))
                    {
                        string json = reader.ReadToEnd();
                        ModInfos? modInfo = JsonSerializer.Deserialize<ModInfos>(json, _jsonSerializerOptions);
                        if (modInfo == null)
                            continue;

                        var iconEntry = modArchive.GetEntry("modicon.png");
                        if (iconEntry != null)
                        {
                            using var stream = iconEntry.Open();
                            using var ms = new MemoryStream();
                            stream.CopyTo(ms);
                            ms.Position = 0;

                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.StreamSource = ms;
                            bitmap.EndInit();
                            bitmap.Freeze();

                            modInfo.Image = bitmap;
                        }
                        else
                        {
                            modInfo.Image = defaultImage;
                        }

                        modInfo.ArchiveName = Path.GetFileName(file);
                        installedMods[modInfo.ModId] = modInfo;
                    }
                }
            }

            modpackInfos.Mods = installedMods;
            return modpackInfos;
        }

        public async Task AddMod(ModpackInfos modpackInfos, ReleaseInfosApi releaseInfos)
        {
            string path = Path.Combine(_configurationService.AppConfig.ModpacksPath, modpackInfos.FolderName, "Mods");

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (var downloadStream = await new HttpClient().GetStreamAsync(releaseInfos.MainFile))
                {
                    var filepath = Path.Combine(path, releaseInfos.Filename);
                    using (var fileStream = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await downloadStream.CopyToAsync(fileStream);
                        await fileStream.FlushAsync();
                        fileStream.Close();
                    }
                }
            } 
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while downloading or installing the mod.", "Installation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RemoveMod(ModpackInfos modpackInfos, ModInfos modInfos)
        {
            string path = Path.Combine(_configurationService.AppConfig.ModpacksPath, modpackInfos.FolderName, "Mods", modInfos.ArchiveName ?? "");
            if (File.Exists(path))
            {
                File.Delete(path);
                modpackInfos.Mods?.Remove(modInfos.ModId);
            }
            else
            {
                MessageBox.Show("Can't locate the mod archive.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
