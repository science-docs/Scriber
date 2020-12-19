using Scriber.Engine;
using Scriber.Layout.Document;
using System.Collections.Generic;

namespace Scriber.Variables
{
    [Package]
    public static class LabelVariables
    {
        public static DocumentLocal<Dictionary<string, TableElement>> Labels { get; } = DocumentLocal.Create<Dictionary<string, TableElement>>();
        [Variable("Counter.Footnote")]
        public static DocumentLocal<int> FootnoteCounter { get; } = new DocumentLocal<int>(0);
    }
}
