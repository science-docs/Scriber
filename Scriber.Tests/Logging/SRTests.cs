using Scriber.Logging;
using System;
using Xunit;

namespace Scriber.Tests.Logging
{
    public class SRTests
    {
        [Fact]
        public void GetNullNameException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                SR.Get(null);
            });
            Assert.Equal("name", ex.ParamName);
        }

        [Fact]
        public void GetNullArgsException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                SR.Get(SRID.IsInterface, null);
            });
            Assert.Equal("args", ex.ParamName);
        }

        [Fact]
        public void GetWithoutParameters()
        {
            var objectCreated = SR.Get(SRID.IsInterface);
            Assert.NotNull(objectCreated);
        }

        [Fact]
        public void GetWithParameters()
        {
            var objectCreated = SR.Get(SRID.TransformSuccess, "TypeA", "TypeB");
            Assert.Contains("TypeA", objectCreated);
            Assert.Contains("TypeB", objectCreated);
        }

        [Fact]
        public void GetException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                SR.Get("not-existing");
            });
            Assert.Equal("name", ex.ParamName);
        }

        [Fact]
        public void MatchesNullNameException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                SR.Matches(null, "Some Message");
            });
            Assert.Equal("name", ex.ParamName);
        }

        [Fact]
        public void MatchesNullMessageException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                SR.Matches(SRID.IsInterface, null);
            });
            Assert.Equal("message", ex.ParamName);
        }

        [Fact]
        public void MatchesSuccess()
        {
            var objectCreated = SR.Get(SRID.ObjectCreated, "object-type");
            Assert.True(SR.Matches(SRID.ObjectCreated, objectCreated));
        }

        [Fact]
        public void MatchesFailure()
        {
            Assert.False(SR.Matches(SRID.ObjectCreated, "Some Message"));
        }

        [Fact]
        public void MatchesException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                SR.Matches("not-existing", "Some Message");
            });
            Assert.Equal("name", ex.ParamName);
        }
    }
}
