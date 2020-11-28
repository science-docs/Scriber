using Scriber.Language;
using Scriber.Language.Syntax;
using Scriber.Tests.Fixture;
using Scriber.Text;
using System;
using System.Reflection;
using Xunit;

namespace Scriber.Engine.Tests
{
    public class ObjectCreatorTests
    {
        private readonly CompilerState state = CompilerStateFixtures.ReflectionLoaded();
        private ObjectCreator Default => new ObjectCreator(E, state);
        private ParameterInfo SimpleParameter => typeof(SimpleObject).GetMethod("SimpleMethod").GetParameters()[0];
        private ParameterInfo OverrideParameter => typeof(SimpleObject).GetMethod("OverrideMethod").GetParameters()[0];
        private ObjectSyntax E => new ObjectSyntax();
        private FieldSyntax Field => new FieldSyntax();

        [Fact]
        public void NullElementConstructionException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                new ObjectCreator(null, CompilerStateFixtures.ReflectionLoaded());
            });
            Assert.Equal("origin", ex.ParamName);
        }

        [Fact]
        public void NullStateConstructionException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                new ObjectCreator(E, null);
            });
            Assert.Equal("compilerState", ex.ParamName);
        }

        [Fact]
        public void NullParameterCreateException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                Default.Create((ParameterInfo)null);
            });
            Assert.Equal("parameter", ex.ParamName);
        }

        [Fact]
        public void NullPropertyCreateException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                Default.Create((PropertyInfo)null);
            });
            Assert.Equal("property", ex.ParamName);
        }

        [Fact]
        public void NullTypeCreateException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                Default.Create(null, null);
            });
            Assert.Equal("defaultType", ex.ParamName);
        }

        [Fact]
        public void CreateWithSimpleParameterInfo()
        {
            var creator = Default;
            creator.Fields.Add(new ObjectField(Field, "A", new Argument(E, "123")));
            var simple = creator.Create(SimpleParameter);
            Assert.IsType<SimpleObject>(simple);
            Assert.Equal(123, ((SimpleObject)simple).A);
        }

        [Fact]
        public void CreateWithPropertyInfo()
        {
            var creator = Default;
            creator.Fields.Add(new ObjectField(Field, "Other", new Argument(E, Default)));
            var property = typeof(SimpleObject).GetProperty("Other");
            var nestedObject = creator.Create(property);
            Assert.IsType<SimpleObject>(nestedObject);
        }

        [Fact]
        public void CreateDynamicObject()
        {
            var creator = Default;
            var nested = Default;
            creator.Fields.Add(new ObjectField(Field, "nested", new Argument(E, nested)));
            nested.Fields.Add(new ObjectField(Field, "key", new Argument(E, "value")));

            dynamic top = creator.Create(typeof(object), null);
            Assert.NotNull(top);
            var nestedVariables = top.nested;
            Assert.NotNull(nestedVariables);
            var value = nestedVariables.key;
            Assert.IsType<string>(value);
            Assert.Equal("value", value);
        }

        [Fact]
        public void CreateDynamicObjectArray()
        {
            var arrayObject = Default;
            arrayObject.Fields.Add(new ObjectField(Field, "key", new Argument(E, "value")));
            var array = new ObjectArray(new ArraySyntax(), state, new Argument[] {
                new Argument(Field, "a"),
                new Argument(Field, arrayObject)
            });
            var creator = Default;
            creator.Fields.Add(new ObjectField(Field, "array", new Argument(E, array)));

            dynamic top = creator.Create(typeof(object), null);
            Assert.NotNull(top);
            var arr = top.array;
            Assert.NotNull(arr);
            var value = arr[0];
            Assert.IsType<string>(value);
            Assert.Equal("a", value);
            var arrayEntry = arr[1];
            var entryValue = arrayEntry.key;
            Assert.IsType<string>(entryValue);
            Assert.Equal("value", entryValue);
        }


        [Fact]
        public void CreateWithOverrideParameterInfo()
        {
            var creator = Default;
            creator.TypeName = "OverrideObject";
            creator.Fields.Add(new ObjectField(Field, "b", new Argument(E, "override")));
            var simple = creator.Create(OverrideParameter);
            Assert.IsType<OverrideObject>(simple);
            Assert.Equal("override", ((SimpleObject)simple).B);
        }

        [Fact]
        public void CreateWithArgumentType()
        {
            var creator = Default;
            creator.Fields.Add(new ObjectField(Field, "A", new Argument(E, "123")));
            var simple = creator.Create(SimpleParameter);
            Assert.IsType<SimpleObject>(simple);
            Assert.Equal(123, ((SimpleObject)simple).A);
        }

        [Fact]
        public void CreateWithGenericArgument()
        {
            var creator = Default;
            creator.Fields.Add(new ObjectField(Field, "A", new Argument<string>(E, "123")));
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
            creator.Fields.Add(new ObjectField(Field, "unknown", new Argument(E, "unknown")));
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
