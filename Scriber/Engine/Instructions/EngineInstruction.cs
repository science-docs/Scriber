using Scriber.Language.Syntax;
using System;
using System.Collections.Generic;

namespace Scriber.Engine.Instructions
{
    public interface IEngineInstruction<T> where T : SyntaxNode
    {
        object? Execute(CompilerState state, T node);
    }

    public static class EngineInstruction
    {
        private static readonly TextInstruction textInstruction = new TextInstruction();
        private static readonly BlockInstruction blockInstruction = new BlockInstruction();
        private static readonly CommandInstruction commandInstruction = new CommandInstruction();
        private static readonly ArgumentInstruction argumentInstruction = new ArgumentInstruction();
        private static readonly StringLiteralInstruction stringInstruction = new StringLiteralInstruction();

        public static Argument Execute(CompilerState state, SyntaxNode node)
        {
            if (node is null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            var result = node switch
            {
                TextSyntax text => textInstruction.Execute(state, text),
                StringLiteralSyntax stringLiteral => stringInstruction.Execute(state, stringLiteral),
                ArgumentSyntax argumentSyntax => argumentInstruction.Execute(state, argumentSyntax),
                CommandSyntax command => commandInstruction.Execute(state, command),
                ListSyntax list => blockInstruction.Execute(state, list),
                ListSyntax<ListSyntax> listOfList => BuildListOfLists(state, listOfList),
                _ => throw new ArgumentException(),
            };

            return new Argument(node, result);
        }

        private static Argument[] BuildListOfLists(CompilerState state, ListSyntax<ListSyntax> listSyntax)
        {
            var list = new List<Argument>();
            foreach (var child in listSyntax.Children)
            {
                list.Add(Execute(state, child));
            }
            return list.ToArray();
        }
    }

    public abstract class EngineInstruction<T> : IEngineInstruction<T> where T : SyntaxNode
    {
        public abstract object? Execute(CompilerState state, T node);


        

        //public static EngineInstruction<SyntaxNode>? Create(SyntaxNode node)
        //{
        //    if (node is null)
        //    {
        //        throw new ArgumentNullException(nameof(node));
        //    }

        //    switch (node)
        //    {
        //        case TextSyntax text:
        //        //case ElementType.Quotation:
        //            return new TextInstruction(text);
        //        case ElementType.Block:
        //        case ElementType.ExplicitBlock:
        //            return new BlockInstruction(node);
        //        case ElementType.Paragraph:
        //            return new EmptyInstruction(node);
        //        case ElementType.Environment:
        //        case ElementType.Command:
        //            return new CommandInstruction(node);
        //        case ElementType.ObjectArray:
        //            return new ObjectArrayInstruction(node);
        //        case ElementType.ObjectCreation:
        //            return new ObjectCreationInstruction(node);
        //        case ElementType.ObjectField:
        //            return new ObjectFieldInstruction(node);
        //        case ElementType.Null:
        //            return new NullInstruction(node);
        //        case ElementType.Comment:
        //            return null;
        //        default:
        //            throw new ArgumentOutOfRangeException(nameof(node), $"Unknown element type '{node.Type}'.");
        //    }
        //}
    }
}
