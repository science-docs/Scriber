using Scriber.Language;
using Scriber.Layout.Document;
using Scriber.Tests.Fixture;
using System;
using Xunit;

namespace Scriber.Engine.Instructions.Tests
{
    public class ObjectInstructionTests
    {
        //private CompilerState CS => CompilerStateFixtures.ReflectionLoaded();

        //[Fact]
        //public void ConstructorWithTypeElement()
        //{
        //    var parent = new Element(null, ElementType.Block, 0, 0);
        //    var typeElement = new Element(parent, ElementType.Text, 0, 0)
        //    {
        //        Content = "type"
        //    };
        //    var objectCreator = new Element(parent, ElementType.ObjectCreation, 0, 0);

        //    var instruction = new ObjectCreationInstruction(objectCreator);
        //    Assert.Equal(typeElement, instruction.TypeElement);
        //}

        //[Fact]
        //public void ConstructorNullElementException()
        //{
        //    var ex = Assert.Throws<ArgumentNullException>(() =>
        //    {
        //        new ObjectCreationInstruction(null);
        //    });
        //    Assert.Equal("origin", ex.ParamName);
        //}

        //[Fact]
        //public void ConstructorNullParentException()
        //{
        //    var objectCreator = new Element(null, ElementType.ObjectCreation, 0, 0);

        //    var ex = Assert.Throws<ArgumentException>(() =>
        //    {
        //        new ObjectCreationInstruction(objectCreator);
        //    });
        //    Assert.Equal("origin", ex.ParamName);
        //}

        //[Fact]
        //public void ConstructorWithNextSiblingsException()
        //{
        //    var parent = new Element(null, ElementType.Block, 0, 0);
        //    var objectCreator = new Element(parent, ElementType.ObjectCreation, 0, 0);
        //    var errorElement = new Element(parent, ElementType.Text, 0, 0);

        //    Assert.Throws<CompilerException>(() =>
        //    {
        //        new ObjectCreationInstruction(objectCreator);
        //    });
        //}

        //[Fact]
        //public void ConstructorWithMultipleSiblingsException()
        //{
        //    var parent = new Element(null, ElementType.Block, 0, 0);
        //    var typeElement = new Element(parent, ElementType.Text, 0, 0)
        //    {
        //        Content = "type"
        //    };
        //    var objectCreator = new Element(parent, ElementType.ObjectCreation, 0, 0);
        //    var errorElement = new Element(parent, ElementType.Text, 0, 0);

        //    Assert.Throws<CompilerException>(() =>
        //    {
        //        new ObjectCreationInstruction(objectCreator);
        //    });
        //}

        //[Fact]
        //public void ExecuteWithTypeElement()
        //{
        //    var cs = CS;
            
        //    var parent = new Element(null, ElementType.Block, 0, 0);
        //    var typeElement = new Element(parent, ElementType.Text, 0, 0)
        //    {
        //        Content = "type"
        //    };
        //    var objectCreatorElement = new Element(parent, ElementType.ObjectCreation, 0, 0);

        //    cs.Blocks.Current.Objects.Add(new Argument(typeElement, new TextLeaf("type")));
        //    cs.Blocks.Push();

        //    var instruction = new ObjectCreationInstruction(objectCreatorElement);
        //    var objectField = new ObjectField(parent, "key", new Argument(parent, "value"));
        //    var objectCreator = instruction.Execute(cs, new Argument[] { new Argument(parent, objectField) });
        //    Assert.IsType<ObjectCreator>(objectCreator);
        //    var creator = (ObjectCreator)objectCreator;
        //    Assert.Single(creator.Fields);
        //    Assert.Equal(objectField, creator.Fields[0]);
        //    Assert.Equal("type", creator.TypeName);
        //    Assert.Equal(typeElement, creator.TypeElement);
        //}

        //[Fact]
        //public void ExecuteWithMultipleParentElementsException()
        //{
        //    var cs = CS;

        //    var parent = new Element(null, ElementType.Block, 0, 0);
        //    var typeElement = new Element(parent, ElementType.Text, 0, 0)
        //    {
        //        Content = "type"
        //    };
        //    var objectCreatorElement = new Element(parent, ElementType.ObjectCreation, 0, 0);

        //    cs.Blocks.Current.Objects.Add(new Argument(typeElement, new TextLeaf("first")));
        //    cs.Blocks.Current.Objects.Add(new Argument(typeElement, new TextLeaf("second")));
        //    cs.Blocks.Push();

        //    var instruction = new ObjectCreationInstruction(objectCreatorElement);

        //    Assert.Throws<InvalidOperationException>(() =>
        //    {
        //        instruction.Execute(cs, Array.Empty<Argument>());
        //    });
        //}

        //[Fact]
        //public void ExecuteSimple()
        //{
        //    var cs = CS;
        //    var parent = new Element(null, ElementType.Block, 0, 0);
        //    var objectCreatorElement = new Element(parent, ElementType.ObjectCreation, 0, 0);

        //    cs.Blocks.Push();

        //    var instruction = new ObjectCreationInstruction(objectCreatorElement);
        //    var objectCreator = instruction.Execute(cs, Array.Empty<Argument>());
        //    Assert.IsType<ObjectCreator>(objectCreator);
        //}

        //[Fact]
        //public void ExecuteCompilerStateNullException()
        //{
        //    var cs = CS;
        //    var parent = new Element(null, ElementType.Block, 0, 0);
        //    var objectCreatorElement = new Element(parent, ElementType.ObjectCreation, 0, 0);

        //    cs.Blocks.Push();

        //    var instruction = new ObjectCreationInstruction(objectCreatorElement);
        //    var ex = Assert.Throws<ArgumentNullException>(() =>
        //    {
        //        instruction.Execute(null, Array.Empty<Argument>());
        //    });
        //    Assert.Equal("state", ex.ParamName);
        //}

        //[Fact]
        //public void ExecuteArgumentsNullException()
        //{
        //    var cs = CS;
        //    var parent = new Element(null, ElementType.Block, 0, 0);
        //    var objectCreatorElement = new Element(parent, ElementType.ObjectCreation, 0, 0);

        //    cs.Blocks.Push();

        //    var instruction = new ObjectCreationInstruction(objectCreatorElement);
        //    var ex = Assert.Throws<ArgumentNullException>(() =>
        //    {
        //        instruction.Execute(cs, null);
        //    });
        //    Assert.Equal("arguments", ex.ParamName);
        //}
    }
}
