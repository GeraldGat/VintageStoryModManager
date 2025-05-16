using System.IO.Compression;
using System.IO;
using System.Text.Json;
using System.Windows.Media.Imaging;
using System.Windows;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services.Interfaces;

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

        public void AddMod(ModpackInfos modpackInfos, ModInfos modInfos)
        {
            throw new NotImplementedException();
        }

        public void RemoveMod(ModpackInfos modpackInfos, ModInfos modInfos)
        {
            throw new NotImplementedException();
        }
    }
}
