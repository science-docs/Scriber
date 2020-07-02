using Scriber.Layout.Document;
using Scriber.Tests.Fixture;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Scriber.Engine.Instructions.Tests
{
    public class TextInstructionTests
    {
        [Fact]
        public void ConstructorNullOriginException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                new TextInstruction(null);
            });
            Assert.Equal("origin", ex.ParamName);
        }

        [Fact]
        public void ConstructorNullContentException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                new TextInstruction(ElementFixtures.EmptyElement());
            });
            Assert.Equal("origin", ex.ParamName);
        }

        [Fact]
        public void ExecuteSimple()
        {
            var instruction = new TextInstruction(new Language.Element(null, Language.ElementType.Text, 0, 0)
            {
                Content = "text"
            });
            var result = instruction.Execute(null, null);
            Assert.IsType<TextLeaf>(result);
            var text = (TextLeaf)result;
            Assert.Equal("text", text.Content);
        }
    }
}
