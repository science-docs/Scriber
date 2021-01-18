using Scriber.Bibliography.Styling.Specification;
using Scriber.Localization;
using System.Collections.Generic;

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
        public IReadOnlyList<Citation> Input { get; set; }
        public IReadOnlyList<Citation[]> Citations { get; set; }
        public string Result { get; set; }
        public StyleFile Csl { get; set; }
        public string Version { get; set; }

        public string Evaluate(Culture culture = null)
        {
            var processor = new Processor(Csl, LocaleFile.Defaults);
            var run = processor.Cite(culture ?? Culture.Invariant, Input);
            return run.ToPlainText();
        }
    }
}
