using System;
using System.IO;
using System.IO.Abstractions;
using System.Text;
using System.Net.Http;

namespace Scriber.Util
{
    public static class IOUtility
    {
        public static Uri ConvertToUri(this IPath path, string uriPath)
        {
            if (Uri.IsWellFormedUriString(uriPath, UriKind.Absolute))
            {
                return new Uri(uriPath, UriKind.Absolute);
            }
            else
            {
                var builder = new UriBuilder
                {
                    Path = path.GetFullPath(uriPath),
                    Scheme = Uri.UriSchemeFile
                };
                return builder.Uri;
            }
        }

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
            else
            {
                throw new ArgumentOutOfRangeException(nameof(uri), "Specified URI contains no valid scheme");
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
