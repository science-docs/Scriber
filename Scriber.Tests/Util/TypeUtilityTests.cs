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
        [InlineData(typeof(Argument[]), "Argument[]")]
        [InlineData(typeof(Argument<Index>[]), "Argument<Index>[]")]
        public void FormattedName(Type type, string value)
        {
            var result = TypeUtility.FormattedName(type);
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(typeof(Array), true)]
        [InlineData(typeof(object[]), true)]
        [InlineData(typeof(object), false)]
        [InlineData(typeof(Argument), false)]
        [InlineData(typeof(Argument[]), true)]
        [InlineData(typeof(IReadOnlyCollection<object>), true)]
        [InlineData(typeof(IReadOnlyCollection<Argument>), true)]
        [InlineData(typeof(IReadOnlyList<Argument>), true)]
        public void IsArrayTypeTest(Type type, bool expectedResult)
        {
            var result = TypeUtility.IsArrayType(type);
            Assert.Equal(expectedResult, result);
        }
    }
}
