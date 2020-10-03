using Scriber.Layout.Document;
using System.Collections.Generic;

namespace Scriber.Variables
{
    public static class LabelVariables
    {
        public static DocumentLocal<Dictionary<string, TableElement>> Labels { get; } = DocumentLocal.Create<Dictionary<string, TableElement>>();
    }
}
