using Scriber.Localization;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Languages
    {
        [Command("Language")]
        public static void Language(CompilerState state, Argument<string> culture)
        {
            if (!Culture.TryGetCulture(culture.Value, out var result))
            {
                state.Issues.Add(culture.Source, CompilerIssueType.Warning, $"Specified culture '{culture.Value}' could not be found.");
            }
            else
            {
                state.Document.Culture = result;
            }
        }
    }
}
