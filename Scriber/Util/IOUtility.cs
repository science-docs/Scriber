using System;
using System.IO;
using System.IO.Abstractions;
using System.Text;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Scriber.Util
{
    public static class IOUtility
    {
        private const string UriPackScheme = "pack";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"/>
        /// <exception cref="IOException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static string ReadAllText(this IFile file, Uri uri)
        {
            return Encoding.UTF8.GetString(file.ReadAllBytes(uri));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"/>
        /// <exception cref="IOException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static byte[] ReadAllBytes(this IFile file, Uri uri)
        {
            if (uri.IsFile)
            {
                return file.ReadAllBytes(uri.AbsolutePath.Substring(1));
            }
            else if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
            {
                return file.FromCache(uri, e => WebUtility.DownloadBytes(e));
            }
            else if (uri.Scheme == UriPackScheme)
            {
                return file.FindPackResource(uri);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(uri), "Specified URI contains no valid scheme");
            }
        }

        private static readonly Regex PackRegex = new Regex(@"^(?:(?<site>(?:application|siteoforigin)):,,,)?(?:\/(?<asm>[a-zA-Z0-9]+)(;v(?<version>(?:\d\.)*\d))?;component)?(?<path>\/[a-zA-Z0-9_\/\.]*)$", RegexOptions.Compiled);

        public static Assembly? FindPackAssembly(string assemblyName, string? assemblyVersion)
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var asm in loadedAssemblies)
            {
                var name = asm.GetName().Name;
                var version = asm.GetName().Version;

                if (Version.TryParse(assemblyVersion, out var asmVersion))
                {
                    if (version != asmVersion)
                    {
                        continue;
                    }
                }

                if (name == assemblyName)
                {
                    return asm;
                }
            }
            return null;
        }

        public static bool TrySplitPackUri(this Uri uri, out string? site, out string? assembly, out string? version, out string? path)
        {
            var uriPath = uri.AbsolutePath;
            var match = PackRegex.Match(uriPath);
            if (match.Success)
            {
                site = null;
                if (match.Groups.TryGetValue("site", out var siteGroup))
                    site = siteGroup.Value;
                assembly = null;
                if (match.Groups.TryGetValue("asm", out var asmGroup))
                    assembly = asmGroup.Value;
                version = null;
                if (match.Groups.TryGetValue("version", out var versionGroup))
                    version = versionGroup.Value;
                path = "/";
                if (match.Groups.TryGetValue("path", out var pathGroup))
                    path = pathGroup.Value;

                return true;
            }
            else
            {
                site = null;
                assembly = null;
                version = null;
                path = null;
                return false;
            }
        }

        public static byte[] FindPackResource(this IFile file, Uri uri)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (TrySplitPackUri(uri, out var site, out var asm, out var version, out var path))
            {
                path ??= "/";
                Stream? stream;
                if (site == "siteoforigin")
                {
                    var currentDir = file.FileSystem.Directory.GetCurrentDirectory();
                    var fullPath = file.FileSystem.Path.Combine(currentDir, path);
                    if (!file.Exists(fullPath))
                    {
                        throw new FileNotFoundException($"Specified file does not exist", path);
                    }
                    else
                    {
                        stream = file.OpenRead(fullPath);
                    }
                }
                else if (site == null || site == "application")
                {
                    Assembly assembly = typeof(IOUtility).Assembly;
                    if (asm != null)
                    {
                        assembly = FindPackAssembly(asm, version) ?? throw new IOException();
                    }
                    var asmPath = path.TrimStart('/').Replace('/', '.');
                    stream = assembly.GetManifestResourceStream(asmPath) ?? throw new FileNotFoundException($"Specified file does not exist", path); ;
                }
                else
                {
                    throw new ArgumentException("Invalid pack authority. Supported authorities are 'application' and 'siteoforigin'.", nameof(uri));
                }

                using var ms = new MemoryStream();
                stream.CopyTo(ms);
                stream.Dispose();
                return ms.ToArray();
            }
            else
            {
                throw new ArgumentException("Not a valid pack uri.", nameof(uri));
            }
        }

        public static string Hash(Uri uri)
        {
            using System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(uri.ToString());
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public static byte[] FromCache(this IFile file, Uri uri, Func<Uri, byte[]> consumer)
        {
            var tempFolder = file.FileSystem.Path.GetTempPath();
            const string folderName = ".scriber";
            var scriberFolder = file.FileSystem.Path.Combine(tempFolder, folderName);
            file.FileSystem.Directory.CreateDirectory(scriberFolder);
            var hash = Hash(uri) + ".tmp";
            var tempFile = file.FileSystem.Path.Combine(scriberFolder, hash);
            if (file.Exists(tempFile))
            {
                return file.ReadAllBytes(tempFile);
            }
            else
            {
                var contents = consumer(uri);
                file.WriteAllBytes(tempFile, contents);
                return contents;
            }
        }
    }
}
