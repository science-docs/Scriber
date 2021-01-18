using Scriber.Layout.Document;
using System.Collections.Generic;

namespace Scriber.Variables
{
    public static class AcronymVariables
    {
        public static DocumentLocal<Dictionary<string, Paragraph>> Acronyms { get; } = new DocumentLocal<Dictionary<string, Paragraph>>();
        public static DocumentLocal<HashSet<string>> UsedAcronyms { get; } = new DocumentLocal<HashSet<string>>();
    }
}
