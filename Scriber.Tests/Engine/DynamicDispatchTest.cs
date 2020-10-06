using Scriber.Engine;
using Scriber.Tests.Fixture;
using System;
using Xunit;

namespace Scriber.Engine.Tests
{
    public class DynamicDispatchTest
    {
        private readonly CompilerState loaded = CompilerStateFixtures.ReflectionLoaded();

        [Theory]
        [InlineData(typeof(Argument))]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Argument<string>))]
        public void SuccessfullSingleString(Type targetType)
        {
            var element = ElementFixtures.EmptyElement();
            var value = "stringValue";
            var argument = new Argument(element, value);

            var success = DynamicDispatch.IsAssignableFrom(loaded, targetType, null, argument, out var transformed);
            Assert.True(success);
            Assert.IsAssignableFrom(targetType, transformed);
        }

        [Theory]
        [InlineData(typeof(Argument[]))]
        [InlineData(typeof(object[]))]
        [InlineData(typeof(string[]))]
        [InlineData(typeof(Argument<string>[]))]
        [InlineData(typeof(Argument<string[]>))]
        public void SuccessfullArgumentArrayString(Type targetType)
        {
            var element = ElementFixtures.EmptyElement();
            var value = new Argument[] { new Argument(element, "first"), new Argument(element, "second") };
            var argument = new Argument(element, value);

            var success = DynamicDispatch.IsAssignableFrom(loaded, targetType, null, argument, out var transformed);
            Assert.True(success);
            Assert.IsAssignableFrom(targetType, transformed);
        }

        [Theory]
        [InlineData(typeof(Argument[]))]
        [InlineData(typeof(object[]))]
        [InlineData(typeof(string[]))]
        [InlineData(typeof(Argument<string>[]))]
        [InlineData(typeof(Argument<string[]>))]
        public void SuccessfullArrayString(Type targetType)
        {
            var element = ElementFixtures.EmptyElement();
            var value = new string[] { "first", "second" };
            var argument = new Argument(element, value);

            var success = DynamicDispatch.IsAssignableFrom(loaded, targetType, null, argument, out var transformed);
            Assert.True(success);
            Assert.IsAssignableFrom(targetType, transformed);
        }

        [Fact]
        public void FailureNullValue()
        {
            var element = ElementFixtures.EmptyElement();
            var argument = new Argument(element, null);

            var success = DynamicDispatch.IsAssignableFrom(loaded, typeof(string), null, argument, out var transformed);
            Assert.False(success);
            Assert.Null(transformed);
        }
    }
}
