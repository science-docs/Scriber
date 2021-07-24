using Scriber.Tests.Fixture;
using System;
using System.Linq;
using System.Text;
using Xunit;

namespace Scriber.Tests
{
    public class ResourceSetTests
    {
        private readonly ResourceManager rs = new ResourceManager(FileSystemMock.FileSystem);

        [Theory]
        [InlineData("https://www.dummy.com/folder/sample.pdf", "test.png", "https://www.dummy.com/folder/test.png")]
        [InlineData("https://www.dummy.com/folder/sample.pdf", "https://www.dummy.com/item.pdf", "https://www.dummy.com/item.pdf")]
        [InlineData("C:/Test/image.png", "C:/item.pdf", "file:///C:/item.pdf")]
        [InlineData("C:/Test/image.png", "item.pdf", "file:///C:/Test/item.pdf")]
        public void RelativeUriWithResource(string current, string srcPath, string targetUri)
        {
            if (current.StartsWith("C:/") && Environment.OSVersion.Platform == PlatformID.Unix)
            {
                return;
            }

            var uri = new Uri(current);
            var resource = new Resource(uri, Array.Empty<byte>());
            rs.PushResource(resource);
            var relativeUri = rs.RelativeUri(srcPath);
            rs.PopResource();
            Assert.Equal(targetUri, relativeUri.ToString());
        }

        [Theory]
        [InlineData("test.png", "file:///C:/test.png")]
        [InlineData("/test.png", "file:///C:/test.png")]
        [InlineData("./test.png", "file:///C:/test.png")]
        [InlineData("path/to/folder/../test.png", "file:///C:/path/to/test.png")]
        [InlineData("https://www.dummy.com/item.pdf", "https://www.dummy.com/item.pdf")]
        public void RelativeUriWithoutResource(string srcPath, string targetUri)
        {
            if (targetUri.Contains("C:/") && Environment.OSVersion.Platform == PlatformID.Unix)
            {
                targetUri = targetUri.Replace("C:/", "");
            }

            var relativeUri = rs.RelativeUri(srcPath);
            Assert.Equal(targetUri, relativeUri.ToString());
        }

        [Fact]
        public void CurrentResourceCorrect()
        {
            var uri = new Uri("https://www.dummy.com/folder/sample.pdf");
            var resource = new Resource(uri, Array.Empty<byte>());
            Assert.Null(rs.CurrentResource);
            rs.PushResource(resource);
            Assert.NotNull(rs.CurrentResource);
            Assert.Equal(resource, rs.CurrentResource);
            rs.PopResource();
            Assert.Null(rs.CurrentResource);
        }

        [Fact]
        public void RelativeUriNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                rs.RelativeUri(null);
            });
            Assert.Equal("uriPath", ex.ParamName);
        }

        [Theory]
        [InlineData("Test.txt")]
        [InlineData("/Test.txt")]
        [InlineData("./Test.txt")]
        public void GetterSuccess(string filePath)
        {
            var resource = rs.Get(rs.RelativeUri(filePath));
            Assert.NotNull(resource);
        }

        [Fact]
        public void GetterNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                rs.Get(null);
            });
            Assert.Equal("uri", ex.ParamName);
        }

        [Fact]
        public void GetStringTest()
        {
            var path = Environment.OSVersion.Platform == PlatformID.Unix ? "/Test.txt" : "C:/Test.txt";
            Assert.Equal("Hello World!", rs.GetString(rs.RelativeUri(path)));
        }

        [Fact]
        public void GetStringNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                rs.GetString(null);
            });
            Assert.Equal("uri", ex.ParamName);
        }

        [Fact]
        public void GetBytesTest()
        {
            var path = Environment.OSVersion.Platform == PlatformID.Unix ? "/Test.txt" : "C:/Test.txt";

            var utf8bom = new UTF8Encoding(true);
            var bytes = utf8bom.GetBytes("Hello World!");
            var preamble = utf8bom.Preamble;
            Assert.Equal(bytes, rs.GetBytes(rs.RelativeUri(path)).Skip(preamble.Length).ToArray());
        }

        [Fact]
        public void GetBytesNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                rs.GetBytes(null);
            });
            Assert.Equal("uri", ex.ParamName);
        }
    }
}
