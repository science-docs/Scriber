using Scriber.Layout.Document;
using System.Collections.Generic;

namespace Scriber.Variables
{
    public static class AcronymVariables
    {
        public static DocumentLocal<List<(string name, Paragraph full)>> Acronyms { get; } = new DocumentLocal<List<(string name, Paragraph full)>>(() => new List<(string, Paragraph)>());
        public static DocumentLocal<HashSet<string>> UsedAcronyms { get; } = new DocumentLocal<HashSet<string>>(() => new HashSet<string>());
    }
}
