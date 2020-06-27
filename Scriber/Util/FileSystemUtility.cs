using System.IO.Abstractions;

namespace Scriber.Util
{
    public static class FileSystemUtility
    {
        public static IFileInfo? TryFindFile(this IFileSystem fileSystem, string name, params string[] extensions)
        {
            foreach (var extension in extensions)
            {
                var fullName = fileSystem.Path.ChangeExtension(name, extension);
                var fileInfo = fileSystem.FileInfo.FromFileName(fullName);
                if (fileInfo.Exists)
                {
                    return fileInfo;
                }
            }
            return null;
        }
    }
}
