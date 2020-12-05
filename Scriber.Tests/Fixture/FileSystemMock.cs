using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Scriber.Tests.Fixture
{
    public static class FileSystemMock
    {
        public static IFileSystem FileSystem
        {
            get
            {
                const string prefix = "Scriber.Tests.Resources.";
                var asm = Assembly.GetExecutingAssembly();
                var names = asm.GetManifestResourceNames().Where(e => e.StartsWith(prefix)).ToArray();

                var dict = new Dictionary<string, MockFileData>();

                foreach (var name in names)
                {
                    var fileName = name[prefix.Length..].Replace('.', '/');
                    var lastIndex = fileName.LastIndexOf('/');
                    fileName = fileName.Remove(lastIndex, 1).Insert(lastIndex, ".").Replace('_', '.');
                    byte[] bytes;
                    using (var stream = asm.GetManifestResourceStream(name))
                    {
                        using var ms = new MemoryStream();
                        stream.CopyTo(ms);
                        bytes = ms.ToArray();
                    }
                    dict.Add(fileName, new MockFileData(bytes));
                }

                MockFileSystem fileSystem;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    fileSystem = new MockFileSystem(dict, @"C:\");
                }
                else
                {
                    fileSystem = new MockFileSystem(dict, "/");
                }
                return fileSystem;
            }
        }
    }
}
