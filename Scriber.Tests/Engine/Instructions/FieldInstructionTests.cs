using Scriber.Language.Syntax;
using Scriber.Layout.Document;
using Scriber.Tests.Fixture;
using System;
using Xunit;

namespace Scriber.Engine.Instructions.Tests
{
    public class FieldInstructionTests
    {
        private static readonly FieldInstruction fieldInstruction = new FieldInstruction();

        [Fact]
        public void EvaluateFieldNameNullException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                fieldInstruction.Evaluate(null, new FieldSyntax());
            });
            Assert.Equal("field", ex.ParamName);
        }

        [Fact]
        public void EvaluateFieldNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                fieldInstruction.Evaluate(CompilerStateFixtures.Empty(), null);
            });
            Assert.Equal("field", ex.ParamName);
        }

        [Fact]
        public void ExecuteSimple()
        {
            var field = new FieldSyntax
            {
                Name = new NameSyntax
                {
                    Value = "key"
                },
                Value = new ListSyntax
                {
                    new TextSyntax
                    {
                        Text = "value"
                    }
                }
            };
            var objectField = fieldInstruction.Evaluate(CompilerStateFixtures.Empty(), field);
            Assert.IsType<ObjectField>(objectField);
            var castedField = (ObjectField)objectField;
            Assert.Equal("key", castedField.Key);
            var resultArguments = (Argument[])castedField.Argument.Value;
            var paragraph = (Paragraph)resultArguments[0].Value;
            var leaf = (ITextLeaf)paragraph.Leaves[0];
            Assert.Equal("value", leaf.Content);
        }
    }
}
