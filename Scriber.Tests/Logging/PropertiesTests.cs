using Scriber.Logging;
using System.IO;
using Xunit;

namespace Scriber.Tests.Logging
{
    public class PropertiesTests
    {
        private Properties Properties => new Properties();

        [Theory]
        [InlineData("key", "value", "key=value")]
        [InlineData("key", "value", "key = value ")]
        [InlineData("key", "value2", "key=value1\nkey=value2")]
        [InlineData("data", "content", "key=value\ndata=content")]
        public void StringFillSuccess(string key, string value, string content)
        {
            var props = Properties;
            props.Fill(content);
            Assert.Equal(value, props[key]);
        }

        [Fact]
        public void StreamFillSuccess()
        {
            var content = "key=value";
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            sw.WriteLine(content);
            sw.Flush();
            var props = Properties;
            ms.Position = 0;
            props.Fill(ms);
            Assert.Equal("value", props["key"]);
        }

        [Fact]
        public void StreamReaderFillSuccess()
        {
            var content = "key=value";
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            sw.WriteLine(content);
            sw.Flush();
            ms.Position = 0;
            var sr = new StreamReader(ms);
            var props = Properties;
            props.Fill(sr);
            Assert.Equal("value", props["key"]);
        }
    }
}
