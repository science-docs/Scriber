using System;
using System.Net.Http;

namespace Scriber.Util
{
    public static class WebUtility
    {
        public static string UserAgent { get; } = typeof(WebUtility).Assembly.GetName().Version!.ToString();

        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"/>
        public static byte[] DownloadBytes(string url)
        {
            return client.GetByteArrayAsync(url).Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"/>
        public static string DownloadString(string url)
        {
            return client.GetStringAsync(url).Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"/>
        public static byte[] DownloadBytes(Uri url)
        {
            return client.GetByteArrayAsync(url).Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"/>
        public static string DownloadString(Uri url)
        {
            return client.GetStringAsync(url).Result;
        }
    }
}
