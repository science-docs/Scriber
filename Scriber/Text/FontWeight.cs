using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Text
{
    [Flags]
    public enum FontWeight
    {
        Normal = 0,
        Bold = 1,
        Italic = 2,
        BoldItalic = 3,
        Underline = 4,
        Strikeout = 8
    }
}
