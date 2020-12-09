using Scriber.Language.Syntax;
using System;

namespace Scriber.Engine.Instructions
{
    public interface IEngineInstruction<T> where T : SyntaxNode
    {
        object? Evaluate(CompilerState state, T node);
    }

    public static class EngineInstruction
    {
        private static readonly TextInstruction textInstruction = new TextInstruction();
        private static readonly BlockInstruction blockInstruction = new BlockInstruction();
        private static readonly CommandInstruction commandInstruction = new CommandInstruction();
        private static readonly ArgumentInstruction argumentInstruction = new ArgumentInstruction();
        private static readonly StringLiteralInstruction stringInstruction = new StringLiteralInstruction();
        private static readonly ObjectInstruction objectCreationInstruction = new ObjectInstruction();
        private static readonly FieldInstruction fieldInstruction = new FieldInstruction();
        private static readonly ArrayInstruction arrayInstruction = new ArrayInstruction();
        private static readonly PercentInstruction percentInstruction = new PercentInstruction();


        public static Argument Evaluate(CompilerState state, SyntaxNode node)
        {
            if (state is null)
                throw new ArgumentNullException(nameof(state));
            if (node is null)
                throw new ArgumentNullException(nameof(node));
            
            var result = node switch
            {
                TextSyntax text => textInstruction.Evaluate(state, text),
                StringLiteralSyntax stringLiteral => stringInstruction.Evaluate(state, stringLiteral),
                ArgumentSyntax argumentSyntax => argumentInstruction.Evaluate(state, argumentSyntax),
                CommandSyntax command => commandInstruction.Evaluate(state, command),
                ObjectSyntax objectSyntax => objectCreationInstruction.Evaluate(state, objectSyntax),
                FieldSyntax fieldSyntax => fieldInstruction.Evaluate(state, fieldSyntax),
                ArraySyntax arraySyntax => arrayInstruction.Evaluate(state, arraySyntax),
                PercentSyntax percentSyntax => percentInstruction.Evaluate(state, percentSyntax),
                ListSyntax list => blockInstruction.Evaluate(state, list),
                CommentSyntax _ => null,
                _ => throw new ArgumentException($"Evaluation of node type {node.GetType().Name} not implemented.", nameof(node)),
            };

            return new Argument(node, result);
        }
    }

    public abstract class EngineInstruction<T> : IEngineInstruction<T> where T : SyntaxNode
    {
        public abstract object? Evaluate(CompilerState state, T node);
    }
}
