using Scriber.Layout.Document;
using System.Collections.Generic;

namespace Scriber.Variables
{
    public static class LabelVariables
    {
        public static DocumentLocal<Dictionary<string, TableElement>> Labels { get; } = new DocumentLocal<Dictionary<string, TableElement>>();
        public static DocumentLocal<int> FootnoteCounter { get; } = new DocumentLocal<int>(0);
    }
}
