using Scriber.Engine;
using System;
using System.Collections.Generic;
using Xunit;

namespace Scriber.Util.Tests
{
    public class TypeUtilityTests
    {
        [Theory]
        [InlineData(typeof(Argument), nameof(Argument))]
        [InlineData(typeof(Argument<>), "Argument<>")]
        [InlineData(typeof(Argument<Index>), "Argument<Index>")]
        [InlineData(typeof(Argument<List<Index>>), "Argument<List<Index>>")]
        public void FormattedName(Type type, string value)
        {
            var result = TypeUtility.FormattedName(type);
            Assert.Equal(value, result);
        }
    }
}
