using Scriber.Language;
using System;
using Xunit;

namespace Scriber.Engine.Instructions.Tests
{
    public class EngineInstructionTests
    {
        [Theory]
        [InlineData("text", ElementType.Text, typeof(TextInstruction))]
        [InlineData("quotation", ElementType.Quotation, typeof(TextInstruction))]
        [InlineData(null, ElementType.Block, typeof(BlockInstruction))]
        [InlineData(null, ElementType.ExplicitBlock, typeof(BlockInstruction))]
        [InlineData("paragraph", ElementType.Paragraph, typeof(EmptyInstruction))]
        [InlineData("command", ElementType.Command, typeof(CommandInstruction))]
        [InlineData("environment", ElementType.Environment, typeof(EnvironmentInstruction))]
        [InlineData(null, ElementType.ObjectArray, typeof(ObjectArrayInstruction))]
        [InlineData("key", ElementType.ObjectField, typeof(ObjectFieldInstruction))]
        [InlineData(null, ElementType.Null, typeof(NullInstruction))]
        public void CorrectInstructionCreated(string? elementContent, ElementType elementType, Type instructionType)
        {
            var element = new Element(null, elementType, 0, 0)
            {
                Content = elementContent
            };
            var instruction = EngineInstruction.Create(element);
            Assert.IsType(instructionType, instruction);
        }

        [Fact]
        public void ObjectCreationInstructionCreated()
        {
            var parent = new Element(null, ElementType.Block, 0, 0);
            var objectCreation = new Element(parent, ElementType.ObjectCreation, 0, 0);
            var instruction = EngineInstruction.Create(objectCreation);
            Assert.IsType<ObjectCreationInstruction>(instruction);
        }

        [Fact]
        public void NullElementException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                EngineInstruction.Create(null);
            });
            Assert.Equal("element", ex.ParamName);
        }

        [Fact]
        public void NonValidTypeException()
        {
            var element = new Element(null, (ElementType)999, 0, 0);

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                EngineInstruction.Create(element);
            });
            Assert.Equal("element", ex.ParamName);
        }
    }
}
