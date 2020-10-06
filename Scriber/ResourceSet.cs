using Scriber.Util;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace Scriber
{
    public class ResourceSet
    {
        public IFileSystem FileSystem { get; }
        public Resource? CurrentResource
        {
            get
            {
                if (resourceStack.TryPeek(out var result))
                {
                    return result;
                }
                return null;
            }
        }

        private readonly Stack<Resource> resourceStack = new Stack<Resource>();

        public ResourceSet(IFileSystem fileSystem)
        {
            FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public void PushResource(Resource resource)
        {
            resourceStack.Push(resource);
        }

        public void PopResource()
        {
            resourceStack.Pop();
        }

        public Uri RelativeUri(string uriPath)
        {
            if (uriPath is null)
            {
                throw new ArgumentNullException(nameof(uriPath));
            }

            var curResource = CurrentResource;
            // If the specified uri path is well formed (e.g. a full file path or http link)
            // then simply ignore the current resource
            if (Uri.IsWellFormedUriString(uriPath, UriKind.Absolute))
            {
                return new Uri(uriPath, UriKind.Absolute);
            }
            else if (curResource != null)
            {
                var absolutePath = curResource.Uri.AbsoluteUri;
                if (absolutePath.Contains('/'))
                {
                    absolutePath = absolutePath.Substring(0, absolutePath.LastIndexOf('/'));
                }
                absolutePath = FileSystem.Path.Combine(absolutePath, uriPath);
                return new Uri(absolutePath, UriKind.Absolute);
            }
            else // Take local file system file if no resource is specified
            {
                var uriBuilder = new UriBuilder
                {
                    Path = FileSystem.Path.GetFullPath(uriPath),
                    Scheme = Uri.UriSchemeFile
                };
                return uriBuilder.Uri;
            }
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
