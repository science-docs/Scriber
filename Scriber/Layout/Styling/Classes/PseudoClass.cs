using Scriber.Layout.Document;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Scriber.Layout.Styling.Classes
{
    public abstract class PseudoClass
    {
        private static readonly Regex ParserRegex = new Regex(@"^(?<name>[\w-]+)(?:\((?<args>.*)\))?$", RegexOptions.Compiled);
        private static readonly string[] PseudoClasses = { "not", "only-child", "last-child", "first-child", "nth-child", "root" };

        public abstract bool Matches(AbstractElement element);

        public static PseudoClass FromString(string pseudoClass)
        {
            var group = ParserRegex.Match(pseudoClass);

            if (!group.Success)
                throw new ArgumentException();

            var name = group.Groups["name"].Value;
            var argsGroup = group.Groups["args"];

            if (!PseudoClasses.Contains(name))
            {
                throw new ArgumentException($"Pseudo class {name} does not exist.");
            }

            PseudoClass? pseudoClassSelector = name switch
            {
                "root" => new RootPseudoClass(),
                "only-child" => new OnlyChildPseudoClass(),
                "first-child" => new FirstChildPseudoClass(),
                "last-child" => new LastChildPseudoClass(),
                _ => null
            };

            if (pseudoClassSelector != null && argsGroup.Success)
            {
                throw new ArgumentException($"Pseudo class {name} does not support arguments.");
            }
            else if (pseudoClassSelector == null && !argsGroup.Success)
            {
                throw new ArgumentException($"Pseudo class {name} requires arguments.");
            }
            else if (pseudoClassSelector == null)
            {
                pseudoClassSelector = name switch
                {
                    "not" => new NotPseudoClass(StyleSelectorParser.Parse(argsGroup.Value)),
                    "nth-child" => new NthChildPseudoClass(argsGroup.Value),
                    _ => throw new InvalidProgramException()
                };
            }

            return pseudoClassSelector;
        }
    }
}
