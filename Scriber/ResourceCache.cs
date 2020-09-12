using Scriber.Util;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace Scriber
{
    public class ResourceCache
    {
        private readonly Dictionary<string, Resource> cache = new Dictionary<string, Resource>();

        public IFileSystem FileSystem { get; }

        public ResourceCache(IFileSystem fileSystem)
        {
            FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public void Clear()
        {
            cache.Clear();
        }

        public void Clear(Uri uri)
        {
            if (uri is null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            cache.Remove(uri.ToString());
        }

        public Resource Get(Uri uri)
        {
            if (uri is null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            return Get(uri, DefaultResolver);
        }

        public byte[] GetBytes(Uri uri)
        {
            if (uri is null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            return Get(uri).GetContent();
        }

        public string GetString(Uri uri)
        {
            if (uri is null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            return Get(uri).GetContentAsString();
        }

        public Resource Get(Uri uri, Func<Uri, byte[]> resourceResolver)
        {
            if (uri is null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (resourceResolver is null)
            {
                throw new ArgumentNullException(nameof(resourceResolver));
            }

            var uriString = uri.ToString();
            if (cache.TryGetValue(uriString, out var resource))
            {
                return resource;
            }
            else
            {
                var bytes = resourceResolver(uri);
                resource = new Resource(uri, bytes);
                cache[uriString] = resource;
                return resource;
            }
        }

        private byte[] DefaultResolver(Uri uri)
        {
            return FileSystem.File.ReadAllBytes(uri);
        }
    }
}
