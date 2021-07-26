using Scriber.Autocomplete;
using Scriber.Language.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scriber.Editor
{
    public class Completion
    {
        public Environment Environment { get; set; }

        private readonly CommandProposalProvider commandProposals = new();

        public Completion(Environment environment)
        {
            Environment = environment;
        }

        public IEnumerable<Proposal> Get(EditorDocument document, int offset)
        {
            var text = document.Content;

            if (CheckCommand(text, offset, out var commandPrefix))
            {
                var proposals = commandProposals.Propose(Environment.Context.Commands, commandPrefix);
                return proposals;
            } 
            else if (CheckArgument(document, offset, out var argProposals))
            {
                return argProposals;
            }

            return Array.Empty<Proposal>();
        }

        private bool CheckCommand(string input, int offset, out string parsed)
        {
            var sb = new StringBuilder();
            parsed = string.Empty;
            for (int i = offset - 1; i > 0; i--)
            {
                var c = input[i];
                if (c == '@')
                {
                    parsed = sb.ToString();
                    return true;
                }
                else if (char.IsLetterOrDigit(c) || c == '*')
                {
                    sb.Insert(0, c);
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private bool CheckArgument(EditorDocument document, int offset, out IEnumerable<Proposal> proposals)
        {
            proposals = Array.Empty<Proposal>();

            var node = document.ParserResult.NodeAt(offset);
            int index = -1;
            CommandSyntax? commandNode = null;

            while (node != null)
            {
                if (node.Parent is ArgumentSyntax arg)
                {
                    if (arg?.Parent?.Parent is CommandSyntax commandSyntax)
                    {
                        index = commandSyntax.Arguments.IndexOf(arg);
                        commandNode = commandSyntax;
                    }
                    break;
                }
                else if (node.Parent is CommandSyntax commandSyntax)
                {
                    index = 0;
                    commandNode = commandSyntax;
                    break;
                }
                node = node.Parent;
            }

            if (commandNode != null && index >= 0)
            {
                var command = Environment.Context.Commands.Find(commandNode.Name.Value, commandNode.Arguments);
                if (command != null)
                {
                    var parameter = command.Parameters[index];
                    var proposalProvider = parameter.ProposalProvider;

                    if (proposalProvider != null)
                    {
                        proposals = proposalProvider.Propose(Environment.CurrentState);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
