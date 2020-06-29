using Scriber.Language;
using Scriber.Tests.Fixture;
using Scriber.Text;
using System;
using System.Reflection;
using Xunit;

namespace Scriber.Engine.Tests
{
    public class ObjectCreatorTests
    {
        private ObjectCreator Default => new ObjectCreator(E, CompilerStateFixtures.ReflectionLoaded());
        private ParameterInfo SimpleParameter => typeof(SimpleObject).GetMethod("SimpleMethod")!.GetParameters()[0];
        private ParameterInfo OverrideParameter => typeof(SimpleObject).GetMethod("OverrideMethod")!.GetParameters()[0];
        private Element E => ElementFixtures.EmptyElement();
        

        [Fact]
        public void CreateWithSimpleParameterInfo()
        {
            var creator = Default;
            creator.Fields.Add(new ObjectField(E, "A", new Argument(E, "123")));
            var simple = creator.Create(SimpleParameter);
            Assert.IsType<SimpleObject>(simple);
            Assert.Equal(123, ((SimpleObject)simple).A);
        }

        [Fact]
        public void CreateWithPropertyInfo()
        {
            var creator = Default;
            creator.Fields.Add(new ObjectField(E, "Other", new Argument(E, Default)));
            var property = typeof(SimpleObject).GetProperty("Other")!;
            var nestedObject = creator.Create(property);
            Assert.IsType<SimpleObject>(nestedObject);
        }

        [Fact]
        public void CreateDocumentVariables()
        {
            var creator = Default;
            var nested = Default;
            creator.Fields.Add(new ObjectField(E, "nested", new Argument(E, nested)));
            nested.Fields.Add(new ObjectField(E, "key", new Argument(E, "value")));

            var top = creator.Create(typeof(DocumentVariable), null) as DocumentVariable;
            Assert.NotNull(top);
            var nestedVariables = top!["nested"];
            Assert.NotNull(nestedVariables);
            var value = nestedVariables["key"];
            Assert.Equal("value", value.GetValue<string>());
        }

        [Fact]
        public void CreateWithOverrideParameterInfo()
        {
            var creator = Default;
            creator.TypeName = "OverrideObject";
            creator.Fields.Add(new ObjectField(E, "b", new Argument(E, "override")));
            var simple = creator.Create(OverrideParameter);
            Assert.IsType<OverrideObject>(simple);
            Assert.Equal("override", ((SimpleObject)simple).B);
        }

        [Fact]
        public void CreateWithArgumentType()
        {
            var creator = Default;
            creator.Fields.Add(new ObjectField(E, "A", new Argument(E, "123")));
            var simple = creator.Create(SimpleParameter);
            Assert.IsType<SimpleObject>(simple);
            Assert.Equal(123, ((SimpleObject)simple).A);
        }

        [Fact]
        public void CreateWithGenericArgument()
        {
            var creator = Default;
            creator.Fields.Add(new ObjectField(E, "A", new Argument<string>(E, "123")));
            var simple = creator.Create(typeof(Argument<SimpleObject>), null);
            Assert.IsType<Argument<SimpleObject>>(simple);
            Assert.Equal(123, ((Argument<SimpleObject>)simple).Value.A);
        }

        [Fact]
        public void CreateWithParameterInfoTypeNameException()
        {
            var creator = Default;
            creator.TypeName = "WrongOverrideObject";
            Assert.Throws<CompilerException>(() =>
            {
                creator.Create(OverrideParameter);
            });
        }

        [Fact]
        public void UnmatchedFieldWarning()
        {
            var creator = Default;
            creator.Fields.Add(new ObjectField(E, "unknown", new Argument(E, "unknown")));
            creator.Create(SimpleParameter);
            Assert.Single(creator.CompilerState.Issues);
        }

        [Theory]
        [InlineData(typeof(Type))]
        [InlineData(typeof(IArrangingStrategy))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Type[]))]
        [InlineData(typeof(FontStyle))]
        [InlineData(typeof(Argument))]
        [InlineData(typeof(Action))]
        public void TypeValidationException(Type type)
        {
            Assert.Throws<CompilerException>(() =>
            {
                Default.Create(type, null);
            });
        }
    }
}
