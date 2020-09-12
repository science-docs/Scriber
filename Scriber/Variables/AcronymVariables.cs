using System.Collections.Generic;

namespace Scriber.Variables
{
    public static class AcronymVariables
    {
        public static DocumentLocal<List<(string name, string full)>> Acronyms { get; } = new DocumentLocal<List<(string name, string full)>>(() => new List<(string, string)>());
        public static DocumentLocal<HashSet<string>> UsedAcronyms { get; } = new DocumentLocal<HashSet<string>>(() => new HashSet<string>());
    }
}
