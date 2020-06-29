using Scriber.Language;
using Scriber.Tests.Fixture;
using System;
using Xunit;

namespace Scriber.Engine.Tests
{
    public class ArgumentTests
    {
        private Element E => ElementFixtures.EmptyElement();

        [Theory]
        [InlineData(typeof(Argument<string>), typeof(string))]
        [InlineData(typeof(Argument<string[]>), typeof(string[]))]
        [InlineData(typeof(Argument<Argument<string>>), typeof(Argument<string>))]
        public void TrueArgumentType(Type argumentType, Type genericArgumentType)
        {
            Assert.True(Argument.IsArgumentType(argumentType, out var genericType));
            Assert.Equal(genericArgumentType, genericType);
        }

        [Fact]
        public void FalseArgumentType()
        {
            Assert.False(Argument.IsArgumentType(typeof(string)));
        }

        [Fact]
        public void NullArgumentTypeException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Argument.IsArgumentType(null);
            });
        }

        [Fact]
        public void NullArgumentOutTypeException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Argument.IsArgumentType(null, out var _);
            });
        }

        [Fact]
        public void FlattenSingle()
        {
            var arg = new Argument(E, 1);
            var flattened = arg.Flatten();
            Assert.Single(flattened);
            Assert.Equal(1, flattened[0].Value);
        }

        [Fact]
        public void FlattenArguments()
        {
            var arg = new Argument(E, new Argument[] {
                new Argument(E, 1),
                new Argument(E, 2)
            });
            var flattened = arg.Flatten();
            Assert.Equal(2, flattened.Length);
            Assert.Equal(1, flattened[0].Value);
            Assert.Equal(2, flattened[1].Value);
        }

        [Fact]
        public void FlattenSimple()
        {
            var arg = new Argument(E, new int[] { 1, 2 });
            var flattened = arg.Flatten();
            Assert.Equal(2, flattened.Length);
            Assert.Equal(1, flattened[0].Value);
            Assert.Equal(2, flattened[1].Value);
        }

        [Fact]
        public void FlattenNested()
        {
            var arg = new Argument(E, new Argument[] {
                new Argument(E, new int[] { 1, 2 }),
                new Argument(E, 3)
            });
            var flattened = arg.Flatten();
            Assert.Equal(3, flattened.Length);
            Assert.Equal(1, flattened[0].Value);
            Assert.Equal(2, flattened[1].Value);
            Assert.Equal(3, flattened[2].Value);
        }

        [Fact]
        public void GenericArgument()
        {
            var arg = new Argument<string>(E, "value");
            Assert.IsType<string>(arg.Value);
            Assert.Equal("value", arg.Value);
        }
    }
}
