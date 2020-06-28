#region PDFsharp - A .NET library for processing PDF
//
// Authors:
//   Stefan Lange
//
// Copyright (c) 2005-2016 empira Software GmbH, Cologne Area (Germany)
//
// http://www.PdfSharpCore.com
// http://sourceforge.net/projects/pdfsharp
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
#endregion

namespace Scriber.Text
{
    public enum KnownColor
    {
        /// <summary>A pre-defined color.</summary>
        AliceBlue = 0,

        /// <summary>A pre-defined color.</summary>
        AntiqueWhite = 1,

        /// <summary>A pre-defined color.</summary>
        Aqua = 2,

        /// <summary>A pre-defined color.</summary>
        Aquamarine = 3,

        /// <summary>A pre-defined color.</summary>
        Azure = 4,

        /// <summary>A pre-defined color.</summary>
        Beige = 5,

        /// <summary>A pre-defined color.</summary>
        Bisque = 6,

        /// <summary>A pre-defined color.</summary>
        Black = 7,

        /// <summary>A pre-defined color.</summary>
        BlanchedAlmond = 8,

        /// <summary>A pre-defined color.</summary>
        Blue = 9,

        /// <summary>A pre-defined color.</summary>
        BlueViolet = 10,

        /// <summary>A pre-defined color.</summary>
        Brown = 11,

        /// <summary>A pre-defined color.</summary>
        BurlyWood = 12,

        /// <summary>A pre-defined color.</summary>
        CadetBlue = 13,

        /// <summary>A pre-defined color.</summary>
        Chartreuse = 14,

        /// <summary>A pre-defined color.</summary>
        Chocolate = 15,

        /// <summary>A pre-defined color.</summary>
        Coral = 16,

        /// <summary>A pre-defined color.</summary>
        CornflowerBlue = 17,

        /// <summary>A pre-defined color.</summary>
        Cornsilk = 18,

        /// <summary>A pre-defined color.</summary>
        Crimson = 19,

        /// <summary>A pre-defined color.</summary>
        Cyan = 20,

        /// <summary>A pre-defined color.</summary>
        DarkBlue = 21,

        /// <summary>A pre-defined color.</summary>
        DarkCyan = 22,

        /// <summary>A pre-defined color.</summary>
        DarkGoldenrod = 23,

        /// <summary>A pre-defined color.</summary>
        DarkGray = 24,

        /// <summary>A pre-defined color.</summary>
        DarkGreen = 25,

        /// <summary>A pre-defined color.</summary>
        DarkKhaki = 26,

        /// <summary>A pre-defined color.</summary>
        DarkMagenta = 27,

        /// <summary>A pre-defined color.</summary>
        DarkOliveGreen = 28,

        /// <summary>A pre-defined color.</summary>
        DarkOrange = 29,

        /// <summary>A pre-defined color.</summary>
        DarkOrchid = 30,

        /// <summary>A pre-defined color.</summary>
        DarkRed = 31,

        /// <summary>A pre-defined color.</summary>
        DarkSalmon = 32,

        /// <summary>A pre-defined color.</summary>
        DarkSeaGreen = 33,

        /// <summary>A pre-defined color.</summary>
        DarkSlateBlue = 34,

        /// <summary>A pre-defined color.</summary>
        DarkSlateGray = 35,

        /// <summary>A pre-defined color.</summary>
        DarkTurquoise = 36,

        /// <summary>A pre-defined color.</summary>
        DarkViolet = 37,

        /// <summary>A pre-defined color.</summary>
        DeepPink = 38,

        /// <summary>A pre-defined color.</summary>
        DeepSkyBlue = 39,

        /// <summary>A pre-defined color.</summary>
        DimGray = 40,

        /// <summary>A pre-defined color.</summary>
        DodgerBlue = 41,

        /// <summary>A pre-defined color.</summary>
        Firebrick = 42,

        /// <summary>A pre-defined color.</summary>
        FloralWhite = 43,

        /// <summary>A pre-defined color.</summary>
        ForestGreen = 44,

        /// <summary>A pre-defined color.</summary>
        Fuchsia = 45,

        /// <summary>A pre-defined color.</summary>
        Gainsboro = 46,

        /// <summary>A pre-defined color.</summary>
        GhostWhite = 47,

        /// <summary>A pre-defined color.</summary>
        Gold = 48,

        /// <summary>A pre-defined color.</summary>
        Goldenrod = 49,

        /// <summary>A pre-defined color.</summary>
        Gray = 50,

        /// <summary>A pre-defined color.</summary>
        Green = 51,

        /// <summary>A pre-defined color.</summary>
        GreenYellow = 52,

        /// <summary>A pre-defined color.</summary>
        Honeydew = 53,

        /// <summary>A pre-defined color.</summary>
        HotPink = 54,

        /// <summary>A pre-defined color.</summary>
        IndianRed = 55,

        /// <summary>A pre-defined color.</summary>
        Indigo = 56,

        /// <summary>A pre-defined color.</summary>
        Ivory = 57,

        /// <summary>A pre-defined color.</summary>
        Khaki = 58,

        /// <summary>A pre-defined color.</summary>
        Lavender = 59,

        /// <summary>A pre-defined color.</summary>
        LavenderBlush = 60,

        /// <summary>A pre-defined color.</summary>
        LawnGreen = 61,

        /// <summary>A pre-defined color.</summary>
        LemonChiffon = 62,

        /// <summary>A pre-defined color.</summary>
        LightBlue = 63,

        /// <summary>A pre-defined color.</summary>
        LightCoral = 64,

        /// <summary>A pre-defined color.</summary>
        LightCyan = 65,

        /// <summary>A pre-defined color.</summary>
        LightGoldenrodYellow = 66,

        /// <summary>A pre-defined color.</summary>
        LightGray = 67,

        /// <summary>A pre-defined color.</summary>
        LightGreen = 68,

        /// <summary>A pre-defined color.</summary>
        LightPink = 69,

        /// <summary>A pre-defined color.</summary>
        LightSalmon = 70,

        /// <summary>A pre-defined color.</summary>
        LightSeaGreen = 71,

        /// <summary>A pre-defined color.</summary>
        LightSkyBlue = 72,

        /// <summary>A pre-defined color.</summary>
        LightSlateGray = 73,

        /// <summary>A pre-defined color.</summary>
        LightSteelBlue = 74,

        /// <summary>A pre-defined color.</summary>
        LightYellow = 75,

        /// <summary>A pre-defined color.</summary>
        Lime = 76,

        /// <summary>A pre-defined color.</summary>
        LimeGreen = 77,

        /// <summary>A pre-defined color.</summary>
        Linen = 78,

        /// <summary>A pre-defined color.</summary>
        Magenta = 79,

        /// <summary>A pre-defined color.</summary>
        Maroon = 80,

        /// <summary>A pre-defined color.</summary>
        MediumAquamarine = 81,

        /// <summary>A pre-defined color.</summary>
        MediumBlue = 82,

        /// <summary>A pre-defined color.</summary>
        MediumOrchid = 83,

        /// <summary>A pre-defined color.</summary>
        MediumPurple = 84,

        /// <summary>A pre-defined color.</summary>
        MediumSeaGreen = 85,

        /// <summary>A pre-defined color.</summary>
        MediumSlateBlue = 86,

        /// <summary>A pre-defined color.</summary>
        MediumSpringGreen = 87,

        /// <summary>A pre-defined color.</summary>
        MediumTurquoise = 88,

        /// <summary>A pre-defined color.</summary>
        MediumVioletRed = 89,

        /// <summary>A pre-defined color.</summary>
        MidnightBlue = 90,

        /// <summary>A pre-defined color.</summary>
        MintCream = 91,

        /// <summary>A pre-defined color.</summary>
        MistyRose = 92,

        /// <summary>A pre-defined color.</summary>
        Moccasin = 93,

        /// <summary>A pre-defined color.</summary>
        NavajoWhite = 94,

        /// <summary>A pre-defined color.</summary>
        Navy = 95,

        /// <summary>A pre-defined color.</summary>
        OldLace = 96,

        /// <summary>A pre-defined color.</summary>
        Olive = 97,

        /// <summary>A pre-defined color.</summary>
        OliveDrab = 98,

        /// <summary>A pre-defined color.</summary>
        Orange = 99,

        /// <summary>A pre-defined color.</summary>
        OrangeRed = 100,

        /// <summary>A pre-defined color.</summary>
        Orchid = 101,

        /// <summary>A pre-defined color.</summary>
        PaleGoldenrod = 102,

        /// <summary>A pre-defined color.</summary>
        PaleGreen = 103,

        /// <summary>A pre-defined color.</summary>
        PaleTurquoise = 104,

        /// <summary>A pre-defined color.</summary>
        PaleVioletRed = 105,

        /// <summary>A pre-defined color.</summary>
        PapayaWhip = 106,

        /// <summary>A pre-defined color.</summary>
        PeachPuff = 107,

        /// <summary>A pre-defined color.</summary>
        Peru = 108,

        /// <summary>A pre-defined color.</summary>
        Pink = 109,

        /// <summary>A pre-defined color.</summary>
        Plum = 110,

        /// <summary>A pre-defined color.</summary>
        PowderBlue = 111,

        /// <summary>A pre-defined color.</summary>
        Purple = 112,

        /// <summary>A pre-defined color.</summary>
        Red = 113,

        /// <summary>A pre-defined color.</summary>
        RosyBrown = 114,

        /// <summary>A pre-defined color.</summary>
        RoyalBlue = 115,

        /// <summary>A pre-defined color.</summary>
        SaddleBrown = 116,

        /// <summary>A pre-defined color.</summary>
        Salmon = 117,

        /// <summary>A pre-defined color.</summary>
        SandyBrown = 118,

        /// <summary>A pre-defined color.</summary>
        SeaGreen = 119,

        /// <summary>A pre-defined color.</summary>
        SeaShell = 120,

        /// <summary>A pre-defined color.</summary>
        Sienna = 121,

        /// <summary>A pre-defined color.</summary>
        Silver = 122,

        /// <summary>A pre-defined color.</summary>
        SkyBlue = 123,

        /// <summary>A pre-defined color.</summary>
        SlateBlue = 124,

        /// <summary>A pre-defined color.</summary>
        SlateGray = 125,

        /// <summary>A pre-defined color.</summary>
        Snow = 126,

        /// <summary>A pre-defined color.</summary>
        SpringGreen = 127,

        /// <summary>A pre-defined color.</summary>
        SteelBlue = 128,

        /// <summary>A pre-defined color.</summary>
        Tan = 129,

        /// <summary>A pre-defined color.</summary>
        Teal = 130,

        /// <summary>A pre-defined color.</summary>
        Thistle = 131,

        /// <summary>A pre-defined color.</summary>
        Tomato = 132,

        /// <summary>A pre-defined color.</summary>
        Transparent = 133,

        /// <summary>A pre-defined color.</summary>
        Turquoise = 134,

        /// <summary>A pre-defined color.</summary>
        Violet = 135,

        /// <summary>A pre-defined color.</summary>
        Wheat = 136,

        /// <summary>A pre-defined color.</summary>
        White = 137,

        /// <summary>A pre-defined color.</summary>
        WhiteSmoke = 138,

        /// <summary>A pre-defined color.</summary>
        Yellow = 139,

        /// <summary>A pre-defined color.</summary>
        YellowGreen = 140,
    }

    internal class KnownColorTable
    {
        internal static uint[] ColorTable = InitColorTable();

        public static uint KnownColorToArgb(KnownColor color)
        {
            if (color <= KnownColor.YellowGreen)
                return ColorTable[(int)color];
            return 0;
        }

        public static bool IsKnownColor(uint argb)
        {
            for (int idx = 0; idx < ColorTable.Length; idx++)
            {
                if (ColorTable[idx] == argb)
                    return true;
            }
            return false;
        }

        public static KnownColor GetKnownColor(uint argb)
        {
            for (int idx = 0; idx < ColorTable.Length; idx++)
            {
                if (ColorTable[idx] == argb)
                    return (KnownColor)idx;
            }
            return (KnownColor)(-1);
        }

        private static uint[] InitColorTable()
        {
            // Same values as in GDI+ and System.Windows.Media.XColors
            // Note that Magenta is the same as Fuchsia and Zyan is the same as Aqua.
            uint[] colors = new uint[141];
            colors[0] = 0xFFF0F8FF;  // AliceBlue
            colors[1] = 0xFFFAEBD7;  // AntiqueWhite
            colors[2] = 0xFF00FFFF;  // Aqua
            colors[3] = 0xFF7FFFD4;  // Aquamarine
            colors[4] = 0xFFF0FFFF;  // Azure
            colors[5] = 0xFFF5F5DC;  // Beige
            colors[6] = 0xFFFFE4C4;  // Bisque
            colors[7] = 0xFF000000;  // Black
            colors[8] = 0xFFFFEBCD;  // BlanchedAlmond
            colors[9] = 0xFF0000FF;  // Blue
            colors[10] = 0xFF8A2BE2;  // BlueViolet
            colors[11] = 0xFFA52A2A;  // Brown
            colors[12] = 0xFFDEB887;  // BurlyWood
            colors[13] = 0xFF5F9EA0;  // CadetBlue
            colors[14] = 0xFF7FFF00;  // Chartreuse
            colors[15] = 0xFFD2691E;  // Chocolate
            colors[16] = 0xFFFF7F50;  // Coral
            colors[17] = 0xFF6495ED;  // CornflowerBlue
            colors[18] = 0xFFFFF8DC;  // Cornsilk
            colors[19] = 0xFFDC143C;  // Crimson
            colors[20] = 0xFF00FFFF;  // Cyan
            colors[21] = 0xFF00008B;  // DarkBlue
            colors[22] = 0xFF008B8B;  // DarkCyan
            colors[23] = 0xFFB8860B;  // DarkGoldenrod
            colors[24] = 0xFFA9A9A9;  // DarkGray
            colors[25] = 0xFF006400;  // DarkGreen
            colors[26] = 0xFFBDB76B;  // DarkKhaki
            colors[27] = 0xFF8B008B;  // DarkMagenta
            colors[28] = 0xFF556B2F;  // DarkOliveGreen
            colors[29] = 0xFFFF8C00;  // DarkOrange
            colors[30] = 0xFF9932CC;  // DarkOrchid
            colors[31] = 0xFF8B0000;  // DarkRed
            colors[32] = 0xFFE9967A;  // DarkSalmon
            colors[33] = 0xFF8FBC8B;  // DarkSeaGreen
            colors[34] = 0xFF483D8B;  // DarkSlateBlue
            colors[35] = 0xFF2F4F4F;  // DarkSlateGray
            colors[36] = 0xFF00CED1;  // DarkTurquoise
            colors[37] = 0xFF9400D3;  // DarkViolet
            colors[38] = 0xFFFF1493;  // DeepPink
            colors[39] = 0xFF00BFFF;  // DeepSkyBlue
            colors[40] = 0xFF696969;  // DimGray
            colors[41] = 0xFF1E90FF;  // DodgerBlue
            colors[42] = 0xFFB22222;  // Firebrick
            colors[43] = 0xFFFFFAF0;  // FloralWhite
            colors[44] = 0xFF228B22;  // ForestGreen
            colors[45] = 0xFFFF00FF;  // Fuchsia
            colors[46] = 0xFFDCDCDC;  // Gainsboro
            colors[47] = 0xFFF8F8FF;  // GhostWhite
            colors[48] = 0xFFFFD700;  // Gold
            colors[49] = 0xFFDAA520;  // Goldenrod
            colors[50] = 0xFF808080;  // Gray
            colors[51] = 0xFF008000;  // Green
            colors[52] = 0xFFADFF2F;  // GreenYellow
            colors[53] = 0xFFF0FFF0;  // Honeydew
            colors[54] = 0xFFFF69B4;  // HotPink
            colors[55] = 0xFFCD5C5C;  // IndianRed
            colors[56] = 0xFF4B0082;  // Indigo
            colors[57] = 0xFFFFFFF0;  // Ivory
            colors[58] = 0xFFF0E68C;  // Khaki
            colors[59] = 0xFFE6E6FA;  // Lavender
            colors[60] = 0xFFFFF0F5;  // LavenderBlush
            colors[61] = 0xFF7CFC00;  // LawnGreen
            colors[62] = 0xFFFFFACD;  // LemonChiffon
            colors[63] = 0xFFADD8E6;  // LightBlue
            colors[64] = 0xFFF08080;  // LightCoral
            colors[65] = 0xFFE0FFFF;  // LightCyan
            colors[66] = 0xFFFAFAD2;  // LightGoldenrodYellow
            colors[67] = 0xFFD3D3D3;  // LightGray
            colors[68] = 0xFF90EE90;  // LightGreen
            colors[69] = 0xFFFFB6C1;  // LightPink
            colors[70] = 0xFFFFA07A;  // LightSalmon
            colors[71] = 0xFF20B2AA;  // LightSeaGreen
            colors[72] = 0xFF87CEFA;  // LightSkyBlue
            colors[73] = 0xFF778899;  // LightSlateGray
            colors[74] = 0xFFB0C4DE;  // LightSteelBlue
            colors[75] = 0xFFFFFFE0;  // LightYellow
            colors[76] = 0xFF00FF00;  // Lime
            colors[77] = 0xFF32CD32;  // LimeGreen
            colors[78] = 0xFFFAF0E6;  // Linen
            colors[79] = 0xFFFF00FF;  // Magenta
            colors[80] = 0xFF800000;  // Maroon
            colors[81] = 0xFF66CDAA;  // MediumAquamarine
            colors[82] = 0xFF0000CD;  // MediumBlue
            colors[83] = 0xFFBA55D3;  // MediumOrchid
            colors[84] = 0xFF9370DB;  // MediumPurple
            colors[85] = 0xFF3CB371;  // MediumSeaGreen
            colors[86] = 0xFF7B68EE;  // MediumSlateBlue
            colors[87] = 0xFF00FA9A;  // MediumSpringGreen
            colors[88] = 0xFF48D1CC;  // MediumTurquoise
            colors[89] = 0xFFC71585;  // MediumVioletRed
            colors[90] = 0xFF191970;  // MidnightBlue
            colors[91] = 0xFFF5FFFA;  // MintCream
            colors[92] = 0xFFFFE4E1;  // MistyRose
            colors[93] = 0xFFFFE4B5;  // Moccasin
            colors[94] = 0xFFFFDEAD;  // NavajoWhite
            colors[95] = 0xFF000080;  // Navy
            colors[96] = 0xFFFDF5E6;  // OldLace
            colors[97] = 0xFF808000;  // Olive
            colors[98] = 0xFF6B8E23;  // OliveDrab
            colors[99] = 0xFFFFA500;  // Orange
            colors[100] = 0xFFFF4500;  // OrangeRed
            colors[101] = 0xFFDA70D6;  // Orchid
            colors[102] = 0xFFEEE8AA;  // PaleGoldenrod
            colors[103] = 0xFF98FB98;  // PaleGreen
            colors[104] = 0xFFAFEEEE;  // PaleTurquoise
            colors[105] = 0xFFDB7093;  // PaleVioletRed
            colors[106] = 0xFFFFEFD5;  // PapayaWhip
            colors[107] = 0xFFFFDAB9;  // PeachPuff
            colors[108] = 0xFFCD853F;  // Peru
            colors[109] = 0xFFFFC0CB;  // Pink
            colors[110] = 0xFFDDA0DD;  // Plum
            colors[111] = 0xFFB0E0E6;  // PowderBlue
            colors[112] = 0xFF800080;  // Purple
            colors[113] = 0xFFFF0000;  // Red
            colors[114] = 0xFFBC8F8F;  // RosyBrown
            colors[115] = 0xFF4169E1;  // RoyalBlue
            colors[116] = 0xFF8B4513;  // SaddleBrown
            colors[117] = 0xFFFA8072;  // Salmon
            colors[118] = 0xFFF4A460;  // SandyBrown
            colors[119] = 0xFF2E8B57;  // SeaGreen
            colors[120] = 0xFFFFF5EE;  // SeaShell
            colors[121] = 0xFFA0522D;  // Sienna
            colors[122] = 0xFFC0C0C0;  // Silver
            colors[123] = 0xFF87CEEB;  // SkyBlue
            colors[124] = 0xFF6A5ACD;  // SlateBlue
            colors[125] = 0xFF708090;  // SlateGray
            colors[126] = 0xFFFFFAFA;  // Snow
            colors[127] = 0xFF00FF7F;  // SpringGreen
            colors[128] = 0xFF4682B4;  // SteelBlue
            colors[129] = 0xFFD2B48C;  // Tan
            colors[130] = 0xFF008080;  // Teal
            colors[131] = 0xFFD8BFD8;  // Thistle
            colors[132] = 0xFFFF6347;  // Tomato
            colors[133] = 0x00FFFFFF;  // Transparent
            colors[134] = 0xFF40E0D0;  // Turquoise
            colors[135] = 0xFFEE82EE;  // Violet
            colors[136] = 0xFFF5DEB3;  // Wheat
            colors[137] = 0xFFFFFFFF;  // White
            colors[138] = 0xFFF5F5F5;  // WhiteSmoke
            colors[139] = 0xFFFFFF00;  // Yellow
            colors[140] = 0xFF9ACD32;  // YellowGreen

            return colors;
        }
    }
}
