using Scriber.Tests.Fixture;
using System;
using Xunit;

namespace Scriber.Engine.Tests
{
    public class BlockStackTests
    {
        private BlockStack Stack => new BlockStack();

        [Fact]
        public void PushNullBlockException()
        {
            var stack = Stack;
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                stack.Push((Block)null);
            });
            Assert.Equal("block", ex.ParamName);
        }

        [Fact]
        public void HasDefaultBlock()
        {
            var stack = Stack;
            Assert.NotNull(stack.Current);
            Assert.True(stack.IsRoot);
            Assert.Equal(1, stack.Count);
            Assert.Equal(stack.Root, stack.Current);
        }

        [Fact]
        public void PushCreatesNewBlock()
        {
            var stack = Stack;
            var last = stack.Current;
            stack.Push();
            Assert.Equal(2, stack.Count);
            Assert.NotEqual(last, stack.Current);
            Assert.NotEqual(stack.Root, stack.Current);
            Assert.False(stack.IsRoot);
        }

        [Fact]
        public void PushCreatesNewNamedBlock()
        {
            var stack = Stack;
            stack.Push("Named");
            Assert.Equal("Named", stack.Current.Name);
        }

        [Fact]
        public void PushBlockInstance()
        {
            var stack = Stack;
            var block = new Block("instance");
            stack.Push(block);
            Assert.Equal(block, stack.Current);
            Assert.Equal("instance", stack.Current.Name);
        }

        [Fact]
        public void PopBlock()
        {
            var stack = Stack;
            stack.Push();
            stack.Current.Objects.Add(new Argument(ElementFixtures.EmptyElement(), "value"));
            Assert.Equal(2, stack.Count);
            Assert.Single(stack.Current.Objects);
            stack.Pop();
            Assert.Equal(1, stack.Count);
            Assert.Empty(stack.Current.Objects);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(2, 2)]
        [InlineData(4, 2)]
        public void PeekSuccess(int push, int peek)
        {
            var stack = Stack;
            Block? last = null;
            for (int i = 0; i < push; i++)
            {
                if (i == push - peek)
                {
                    last = stack.Current;
                }
                stack.Push();
            }

            last ??= stack.Current;

            var peeked = stack.Peek(peek);

            Assert.Equal(last, peeked);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(0, -1)]
        [InlineData(2, 4)]
        public void PeekException(int push, int peek)
        {
            var stack = Stack;
            for (int i = 0; i < push; i++)
            {
                stack.Push();
            }

            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                stack.Peek(peek);
            });
        }
    }
}
