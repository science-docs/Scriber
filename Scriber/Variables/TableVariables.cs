using Scriber.Layout.Document;
using System.Collections.Generic;

namespace Scriber.Variables
{
    public static class TableVariables
    {
        public static DocumentLocal<List<TableElement>> TableOfContent { get; } = new DocumentLocal<List<TableElement>>();
        public static DocumentLocal<List<TableElement>> TableOfFigures { get; } = new DocumentLocal<List<TableElement>>();
        public static DocumentLocal<List<TableElement>> TableOfTables { get; } = new DocumentLocal<List<TableElement>>();
    }
}
