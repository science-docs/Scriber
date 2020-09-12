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
                return WebUtility.DownloadBytes(uri);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(uri), "Specified URI contains no valid scheme");
            }
        }
    }
}
