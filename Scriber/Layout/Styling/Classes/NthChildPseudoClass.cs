using Scriber.Layout.Document;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Scriber.Layout.Styling.Classes
{
    public class NthChildPseudoClass : PseudoClass
    {
        private static readonly Regex ParserRegex = new Regex(@"^(?:(?<num>-?\d+)(?:(?<n>n)(?:\s*\+\s*(?<full>\d+))?)?)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public int Step { get; set; }
        public int Offset { get; set; } = 1;

        public NthChildPseudoClass()
        {
        }

        public NthChildPseudoClass(string input)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            input = input.Trim();
            if (input.Equals("odd", StringComparison.InvariantCultureIgnoreCase))
            {
                Step = 2;
                Offset = 1;
            }
            else if (input.Equals("even", StringComparison.InvariantCultureIgnoreCase))
            {
                Step = 2;
                Offset = 0;
            }
            else
            {
                var parserResult = ParserRegex.Match(input);

                if (!parserResult.Success)
                {
                    throw new ArgumentException("Nth-child pseudo class needs a Function in the form of An+B.");
                }

                var numGroup = parserResult.Groups["num"]!;
                var nGroup = parserResult.Groups["n"];
                var fullGroup = parserResult.Groups["full"];

                if (!fullGroup.Success && !nGroup.Success)
                {
                    Step = 0;
                    Offset = int.Parse(numGroup.Value);
                }
                else if (!fullGroup.Success && nGroup.Success)
                {
                    Step = int.Parse(numGroup.Value);
                    Offset = 0;
                }
                else if (fullGroup.Success)
                {
                    Step = int.Parse(numGroup.Value);
                    Offset = int.Parse(fullGroup.Value);
                }
            }
        }

        public override bool Matches(AbstractElement element)
        {
            var parent = element.Parent;
            if (parent == null)
                return false;

            var children = parent.ChildElements().ToList();
            // The position refers to an one based index
            // position = index + 1
            var position = children.IndexOf(element) + 1;

            if (position < Offset || position < 1)
                return false;

            if (Step == 0)
            {
                return position == Offset;
            }
            else
            {
                return (position - Offset) % Step == 0;
            }
        }
    }
}
