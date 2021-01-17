using Scriber.Bibliography.Styling.Specification;
using Scriber.Localization;

namespace Scriber.Bibliography.Styling.Tests
{
    public enum CslTestMode
    {
        Citation,
        Bibliography
    }

    public class CslTestSuite
    {
        public CslTestMode Mode { get; set; }
        public Citation Input { get; set; }
#pragma warning disable CA1819 // Properties should not return arrays
        public Citation[][] Citations { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays
        public string Result { get; set; }
        public StyleFile Csl { get; set; }
        public string Version { get; set; }

        public string Evaluate(Culture culture = null)
        {
            var processor = new Processor(Csl, LocaleFile.Defaults);
            var run = processor.Cite(culture ?? Culture.Invariant, new Citation[] { Input });
            return run.ToPlainText();
        }
    }
}
