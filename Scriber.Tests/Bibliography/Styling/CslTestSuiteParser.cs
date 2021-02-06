using Scriber.Bibliography.Styling.Specification;
using Scriber.Util;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;

using Match = System.Text.RegularExpressions.Match;

namespace Scriber.Bibliography.Styling.Tests
{
    public static class CslTestSuiteParser
    {
        private static readonly Regex ParserRegex = new Regex(@"(?:>>=+\s(?<name>[\w-]+)\s=+>>(?<content>.*?)<<=+\s[\w-]+\s=+<<)+", RegexOptions.Compiled | RegexOptions.Singleline);

        public static CslTestSuite Parse(string input)
        {
            var matches = ParserRegex.Matches(input);
            var suite = new CslTestSuite();

            foreach (Match match in matches)
            {
                var name = match.Groups["name"].Value;
                var content = match.Groups["content"].Value.Trim();

                switch (name)
                {
                    case "MODE":
                        suite.Mode = EnumUtility.ParseEnum<CslTestMode>(content).Value;
                        break;
                    case "INPUT":
                        var array = JsonDocument.Parse(content).RootElement.EnumerateArray();
                        var inputs = new List<Citation>();
                        while (array.MoveNext())
                        {
                            inputs.Add(Citation.FromJson(array.Current));
                        }
                        suite.Input = inputs.ToArray();
                        break;
                    case "RESULT":
                        suite.Result = content;
                        break;
                    case "CSL":
                        suite.Csl = File.Parse<StyleFile>(content);
                        break;
                    case "CITATIONS":
                        break;
                    case "VERSION":
                        suite.Version = content;
                        break;
                }
            }

            return suite;
        }
    }
}
