using Scriber.Language.Syntax;
using Scriber.Layout.Document;
using Scriber.Tests.Fixture;
using System;
using System.Linq;
using Xunit;

namespace Scriber.Engine.Tests
{
    public class ObjectArrayTests
    {
        private ArraySyntax E => new ArraySyntax();
        private CompilerState CS => CompilerStateFixtures.ReflectionLoaded();

        [Fact]
        public void NullElementConstructorException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                new ObjectArray(null, CS, Array.Empty<Argument>());
            });
            Assert.Equal("origin", ex.ParamName);
        }

        [Fact]
        public void NullStateConstructorException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                new ObjectArray(E, null, Array.Empty<Argument>());
            });
            Assert.Equal("compilerState", ex.ParamName);
        }

        [Fact]
        public void NullArgumentsConstructorException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                new ObjectArray(E, CS, null);
            });
            Assert.Equal("objects", ex.ParamName);
        }

        [Fact]
        public void NullTypeException()
        {
            var objectArray = Create();
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                objectArray.Get(null);
            });
            Assert.Equal("type", ex.ParamName);
        }

        [Fact]
        public void ParagraphArray()
        {
            var firstParagraph = Paragraph.FromText("first");
            var secondParagraph = Paragraph.FromText("second");
            var objectArray = Create(firstParagraph, secondParagraph);

            var array = objectArray.Get(typeof(Paragraph));
            var paragraphs = Assert.IsType<Paragraph[]>(array);

            Assert.Equal(firstParagraph, paragraphs[0]);
            Assert.Equal(secondParagraph, paragraphs[1]);
        }

        [Fact]
        public void ObjectCreatorArray()
        {
            var creator = new ObjectCreator(new ObjectSyntax(), CS);
            var objectArray = Create(creator);
            creator.Fields.Add(new ObjectField(new FieldSyntax(), "A", new Argument(E, 123)));

            var array = objectArray.Get(typeof(SimpleObject));
            var objects = Assert.IsType<SimpleObject[]>(array);

            Assert.Equal(123, objects[0].A);
        }

        [Fact]
        public void NullElementArray()
        {
            var objectArray = Create(new object?[] { null });
            var array = objectArray.Get(typeof(SimpleObject));
            var objects = Assert.IsType<SimpleObject[]>(array);

            Assert.Null(objects[0]);
        }

        [Fact]
        public void ConvertedArray()
        {
            var objectArray = Create("1", "2");
            var array = objectArray.Get(typeof(int));
            var objects = Assert.IsType<int[]>(array);

            Assert.Equal(1, objects[0]);
            Assert.Equal(2, objects[1]);
        }

        [Fact]
        public void InvalidConversion()
        {
            var objectArray = Create("Simple");

            Assert.ThrowsAny<CompilerException>(() =>
            {
                objectArray.Get(typeof(SimpleObject));
            });
        }

        [Fact]
        public void InvalidNestedArrayType()
        {
            var objectArray = Create();
            var emptyNested = objectArray.Get(typeof(SimpleObject[]));
            Assert.NotNull(emptyNested);
            Assert.Empty(emptyNested);
        }

        private ObjectArray Create(params object?[] elements)
        {
            return new ObjectArray(E, CS, elements.Select(e => new Argument(E, e)).ToArray());
        }
    }
}
