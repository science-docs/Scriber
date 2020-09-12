using Scriber.Language;
using Scriber.Tests.Fixture;
using System;
using Xunit;

namespace Scriber.Engine.Instructions.Tests
{
    public class ObjectFieldInstructionTests
    {
        [Fact]
        public void ConstructorNullElementException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                new ObjectFieldInstruction(null);
            });
            Assert.Equal("origin", ex.ParamName);
        }

        [Fact]
        public void ConstructorNullContentException()
        {
            var field = new Element(null, ElementType.ObjectField, 0, 0)
            {
                Content = null
            };
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                new ObjectFieldInstruction(field);
            });
            Assert.Equal("origin", ex.ParamName);
        }

        [Fact]
        public void ExecuteNullArgumentsException()
        {
            var field = new Element(null, ElementType.ObjectField, 0, 0)
            {
                Content = "key"
            };
            var instruction = new ObjectFieldInstruction(field);
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                instruction.Execute(null, null);
            });
            Assert.Equal("arguments", ex.ParamName);
        }

        [Fact]
        public void ExecuteMinLengthArgumentsException()
        {
            var field = new Element(null, ElementType.ObjectField, 0, 0)
            {
                Content = "key"
            };
            var instruction = new ObjectFieldInstruction(field);
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                instruction.Execute(null, Array.Empty<Argument>());
            });
            Assert.Equal("arguments", ex.ParamName);
        }

        [Fact]
        public void ExecuteMaxLengthArgumentsException()
        {
            var field = new Element(null, ElementType.ObjectField, 0, 0)
            {
                Content = "key"
            };
            var instruction = new ObjectFieldInstruction(field);
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                instruction.Execute(null, new Argument[2]);
            });
            Assert.Equal("arguments", ex.ParamName);
        }

        [Fact]
        public void ExecuteNullArgumentException()
        {
            var field = new Element(null, ElementType.ObjectField, 0, 0)
            {
                Content = "key"
            };
            var instruction = new ObjectFieldInstruction(field);
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                instruction.Execute(null, new Argument[1]);
            });
            Assert.Equal("arguments", ex.ParamName);
        }

        [Fact]
        public void ExecuteSimple()
        {
            var field = new Element(null, ElementType.ObjectField, 0, 0)
            {
                Content = "key"
            };
            var instruction = new ObjectFieldInstruction(field);
            var objectField = instruction.Execute(null, new Argument[] { new Argument(ElementFixtures.EmptyElement(), "value") });
            Assert.IsType<ObjectField>(objectField);
            var castedField = (ObjectField)objectField;
            Assert.Equal("key", castedField.Key);
            Assert.Equal("value", castedField.Argument.Value);
        }
    }
}
