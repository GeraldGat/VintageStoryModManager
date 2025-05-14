using System.IO.Compression;
using System.IO;

namespace VSModpackManager.Extensions
{
    public static class ZipArchiveExtended
    {
        public static void CreateEntriesFromDirectory(this ZipArchive archive, string sourceDirectory, string entryRoot = "")
        {
            if (!Directory.Exists(sourceDirectory))
                throw new DirectoryNotFoundException($"Folder {sourceDirectory} not found.");

            Directory.GetFiles(sourceDirectory).ToList().ForEach((file) =>
            {
                archive.CreateEntryFromFile(file, Path.Combine(entryRoot, Path.GetRelativePath(sourceDirectory, file)));
            });

            Directory.GetDirectories(sourceDirectory).ToList().ForEach((directory) =>
            {
                string directoryName = Path.GetFileName(directory);
                string newEntryRoot = Path.Combine(entryRoot, directoryName);
                archive.CreateEntry(newEntryRoot + "/");
                archive.CreateEntriesFromDirectory(directory, newEntryRoot);
            });
        }
    }
}
