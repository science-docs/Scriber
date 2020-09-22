using Scriber.Util;
using System;
using System.IO.Abstractions;

namespace Scriber
{
    public class ResourceSet
    {
        public IFileSystem FileSystem { get; }

        public ResourceSet(IFileSystem fileSystem)
        {
            FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
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

        private static Resource Get(Uri uri, Func<Uri, byte[]> resourceResolver)
        {
            if (uri is null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (resourceResolver is null)
            {
                throw new ArgumentNullException(nameof(resourceResolver));
            }

            var bytes = resourceResolver(uri);
            var resource = new Resource(uri, bytes);
            return resource;
        }

        private byte[] DefaultResolver(Uri uri)
        {
            return FileSystem.File.ReadAllBytes(uri);
        }
    }
}
