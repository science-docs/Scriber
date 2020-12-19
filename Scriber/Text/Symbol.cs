using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Text
{
    public enum SymbolType
    {
        Word,
        Math,
        Header
    }

    public class Symbol
    {
        public string Content { get; set; } = string.Empty;
        public int Length => Content.Length;
        public SymbolType Type { get; set; }

        public Symbol(SymbolType type, string content)
        {
            Type = type;
            Content = content;
        }
    }
}
