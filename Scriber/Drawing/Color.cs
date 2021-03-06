﻿#region PDFsharp - A .NET library for processing PDF
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

using Scriber.Util;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Scriber.Drawing
{
    public enum ColorSpace
    {
        /// <summary>
        /// Identifies the RGB color space.
        /// </summary>
        Rgb,

        /// <summary>
        /// Identifies the CMYK color space.
        /// </summary>
        Cmyk,

        /// <summary>
        /// Identifies the gray scale color space.
        /// </summary>
        GrayScale,
    }

    [DebuggerDisplay("clr=(A={A}, R={R}, G={G}, B={B} C={C}, M={M}, Y={Y}, K={K})")]
    public struct Color
    {
        Color(uint argb)
        {
            _cs = ColorSpace.Rgb;
            _a = (byte)((argb >> 24) & 0xff) / 255f;
            _r = (byte)((argb >> 16) & 0xff);
            _g = (byte)((argb >> 8) & 0xff);
            _b = (byte)(argb & 0xff);
            _c = 0;
            _m = 0;
            _y = 0;
            _k = 0;
            _gs = 0;
            RgbChanged();
            //_cs.GetType(); // Suppress warning
        }

        Color(byte alpha, byte red, byte green, byte blue)
        {
            _cs = ColorSpace.Rgb;
            _a = alpha / 255f;
            _r = red;
            _g = green;
            _b = blue;
            _c = 0;
            _m = 0;
            _y = 0;
            _k = 0;
            _gs = 0;
            RgbChanged();
            //_cs.GetType(); // Suppress warning
        }

        Color(double alpha, double cyan, double magenta, double yellow, double black)
        {
            _cs = ColorSpace.Cmyk;
            _a = (float)(alpha > 1 ? 1 : (alpha < 0 ? 0 : alpha));
            _c = (float)(cyan > 1 ? 1 : (cyan < 0 ? 0 : cyan));
            _m = (float)(magenta > 1 ? 1 : (magenta < 0 ? 0 : magenta));
            _y = (float)(yellow > 1 ? 1 : (yellow < 0 ? 0 : yellow));
            _k = (float)(black > 1 ? 1 : (black < 0 ? 0 : black));
            _r = 0;
            _g = 0;
            _b = 0;
            _gs = 0f;
            CmykChanged();
        }

        Color(double cyan, double magenta, double yellow, double black)
            : this(1.0, cyan, magenta, yellow, black)
        { }

        Color(double gray)
        {
            _cs = ColorSpace.GrayScale;
            if (gray < 0)
                _gs = 0;
            else if (gray > 1)
                _gs = 1;
            _gs = (float)gray;

            _a = 1;
            _r = 0;
            _g = 0;
            _b = 0;
            _c = 0;
            _m = 0;
            _y = 0;
            _k = 0;
            GrayChanged();
        }

        internal Color(KnownColor knownColor)
            : this(KnownColorTable.KnownColorToArgb(knownColor))
        { }

        /// <summary>
        /// Creates an XColor structure from a 32-bit ARGB value.
        /// </summary>
        public static Color FromArgb(int argb)
        {
            return new Color((byte)(argb >> 24), (byte)(argb >> 16), (byte)(argb >> 8), (byte)argb);
        }

        /// <summary>
        /// Creates an XColor structure from a 32-bit ARGB value.
        /// </summary>
        public static Color FromArgb(uint argb)
        {
            return new Color((byte)(argb >> 24), (byte)(argb >> 16), (byte)(argb >> 8), (byte)argb);
        }

        // from System.Drawing.Color
        //public static XColor FromArgb(int alpha, Color baseColor);
        //public static XColor FromArgb(int red, int green, int blue);
        //public static XColor FromArgb(int alpha, int red, int green, int blue);
        //public static XColor FromKnownColor(KnownColor color);
        //public static XColor FromName(string name);

        /// <summary>
        /// Creates an XColor structure from the specified 8-bit color values (red, green, and blue).
        /// The alpha value is implicitly 255 (fully opaque).
        /// </summary>
        public static Color FromArgb(int red, int green, int blue)
        {
            CheckByte(red, nameof(red));
            CheckByte(green, nameof(green));
            CheckByte(blue, nameof(blue));
            return new Color(255, (byte)red, (byte)green, (byte)blue);
        }

        /// <summary>
        /// Creates an XColor structure from the four ARGB component (alpha, red, green, and blue) values.
        /// </summary>
        public static Color FromArgb(int alpha, int red, int green, int blue)
        {
            CheckByte(alpha, nameof(alpha));
            CheckByte(red, nameof(red));
            CheckByte(green, nameof(green));
            CheckByte(blue, nameof(blue));
            return new Color((byte)alpha, (byte)red, (byte)green, (byte)blue);
        }

        /// <summary>
        /// Creates an XColor structure from the specified alpha value and color.
        /// </summary>
        public static Color FromArgb(int alpha, Color color)
        {
            color.A = ((byte)alpha) / 255.0;
            return color;
        }

        /// <summary>
        /// Creates an XColor structure from the specified CMYK values.
        /// </summary>
        public static Color FromCmyk(double cyan, double magenta, double yellow, double black)
        {
            return new Color(cyan, magenta, yellow, black);
        }

        /// <summary>
        /// Creates an XColor structure from the specified CMYK values.
        /// </summary>
        public static Color FromCmyk(double alpha, double cyan, double magenta, double yellow, double black)
        {
            return new Color(alpha, cyan, magenta, yellow, black);
        }

        /// <summary>
        /// Creates an XColor structure from the specified gray value.
        /// </summary>
        public static Color FromGrayScale(double grayScale)
        {
            return new Color(grayScale);
        }

        /// <summary>
        /// Creates an XColor from the specified pre-defined color.
        /// </summary>
        public static Color FromKnownColor(KnownColor color)
        {
            return new Color(color);
        }

        public static Color? FromName(string name)
        {
            if (EnumUtility.TryParseEnum<KnownColor>(name, out var knownColor))
            {
                return new Color(knownColor);
            }
            else
            {
                return null;
            }
        }

        private static readonly Regex colorCodeRegex = new Regex("^#([0-9A-Fa-f]{3}(?:[0-9A-Fa-f]{3})?)$", RegexOptions.Compiled);

        public static bool IsColorCode(string code)
        {
            return colorCodeRegex.IsMatch(code);
        }

        public static Color? FromCode(string code)
        {
            var matches = colorCodeRegex.Matches(code);

            if (matches.Count == 1)
            {
                var hex = matches[0].Groups[1].Value;
                hex = DuplicateTriples(hex);
                return FromArgb(ToInt(hex[0..2]), ToInt(hex[2..4]), ToInt(hex[4..6]));
            }
            return null;
        }

        public static bool TryParse(string value, out Color color)
        {
            var nullableColor = FromCode(value) ?? FromName(value);
            color = nullableColor ?? default;
            return nullableColor != null;
        }

        /// <summary>
        /// Gets or sets the color space to be used for PDF generation.
        /// </summary>
        public ColorSpace ColorSpace
        {
            get { return _cs; }
            set
            {
                if (!Enum.IsDefined(typeof(ColorSpace), value))
                    throw new ArgumentException("Specified color space is not a defined value.", nameof(value));
                _cs = value;
            }
        }

        /// <summary>
        /// Indicates whether this XColor structure is uninitialized.
        /// </summary>
        public bool IsEmpty
        {
            get { return this == Empty; }
        }

        /// <summary>
        /// Determines whether the specified object is a Color structure and is equivalent to this 
        /// Color structure.
        /// </summary>
        public override bool Equals(object? obj)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (obj is Color color)
            {
                if (_r == color._r && _g == color._g && _b == color._b &&
                  _c == color._c && _m == color._m && _y == color._y && _k == color._k &&
                  _gs == color._gs)
                {
                    return _a == color._a;
                }
            }
            return false;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyFieldInGetHashCode
            return ((byte)(_a * 255)) ^ _r ^ _g ^ _b;
            // ReSharper restore NonReadonlyFieldInGetHashCode
        }

        /// <summary>
        /// Determines whether two colors are equal.
        /// </summary>
        public static bool operator ==(Color left, Color right)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (left._r == right._r && left._g == right._g && left._b == right._b &&
                left._c == right._c && left._m == right._m && left._y == right._y && left._k == right._k &&
                left._gs == right._gs)
            {
                return left._a == right._a;
            }
            return false;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>
        /// Determines whether two colors are not equal.
        /// </summary>
        public static bool operator !=(Color left, Color right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Gets a value indicating whether this color is a known color.
        /// </summary>
        public bool IsKnownColor
        {
            get { return KnownColorTable.IsKnownColor(Argb); }
        }

        /// <summary>
        /// Gets the hue-saturation-brightness (HSB) hue value, in degrees, for this color.
        /// </summary>
        /// <returns>The hue, in degrees, of this color. The hue is measured in degrees, ranging from 0 through 360, in HSB color space.</returns>
        public double GetHue()
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if ((_r == _g) && (_g == _b))
                return 0;

            double value1 = _r / 255.0;
            double value2 = _g / 255.0;
            double value3 = _b / 255.0;
            double value7 = 0;
            double value4 = value1;
            double value5 = value1;
            if (value2 > value4)
                value4 = value2;

            if (value3 > value4)
                value4 = value3;

            if (value2 < value5)
                value5 = value2;

            if (value3 < value5)
                value5 = value3;

            double value6 = value4 - value5;
            if (value1 == value4)
                value7 = (value2 - value3) / value6;
            else if (value2 == value4)
                value7 = 2f + ((value3 - value1) / value6);
            else if (value3 == value4)
                value7 = 4f + ((value1 - value2) / value6);

            value7 *= 60;
            if (value7 < 0)
                value7 += 360;
            return value7;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>
        /// Gets the hue-saturation-brightness (HSB) saturation value for this color.
        /// </summary>
        /// <returns>The saturation of this color. The saturation ranges from 0 through 1, where 0 is grayscale and 1 is the most saturated.</returns>
        public double GetSaturation()
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            double value1 = _r / 255.0;
            double value2 = _g / 255.0;
            double value3 = _b / 255.0;
            double value7 = 0;
            double value4 = value1;
            double value5 = value1;
            if (value2 > value4)
                value4 = value2;

            if (value3 > value4)
                value4 = value3;

            if (value2 < value5)
                value5 = value2;

            if (value3 < value5)
                value5 = value3;

            if (value4 == value5)
                return value7;

            double value6 = (value4 + value5) / 2;
            if (value6 <= 0.5)
                return (value4 - value5) / (value4 + value5);
            return (value4 - value5) / ((2f - value4) - value5);
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>
        /// Gets the hue-saturation-brightness (HSB) brightness value for this color.
        /// </summary>
        /// <returns>The brightness of this color. The brightness ranges from 0 through 1, where 0 represents black and 1 represents white.</returns>
        public double GetBrightness()
        {
            double value1 = _r / 255.0;
            double value2 = _g / 255.0;
            double value3 = _b / 255.0;
            double value4 = value1;
            double value5 = value1;
            if (value2 > value4)
                value4 = value2;

            if (value3 > value4)
                value4 = value3;

            if (value2 < value5)
                value5 = value2;

            if (value3 < value5)
                value5 = value3;

            return (value4 + value5) / 2;
        }

        ///<summary>
        /// One of the RGB values changed; recalculate other color representations.
        /// </summary>
        void RgbChanged()
        {
            // ReSharper disable LocalVariableHidesMember
            _cs = ColorSpace.Rgb;
            int c = 255 - _r;
            int m = 255 - _g;
            int y = 255 - _b;
            int k = Math.Min(c, Math.Min(m, y));
            if (k == 255)
                _c = _m = _y = 0;
            else
            {
                float black = 255f - k;
                _c = (c - k) / black;
                _m = (m - k) / black;
                _y = (y - k) / black;
            }
            _k = _gs = k / 255f;
            // ReSharper restore LocalVariableHidesMember
        }

        ///<summary>
        /// One of the CMYK values changed; recalculate other color representations.
        /// </summary>
        void CmykChanged()
        {
            _cs = ColorSpace.Cmyk;
            float black = _k * 255;
            float factor = 255f - black;
            _r = (byte)(255 - Math.Min(255f, _c * factor + black));
            _g = (byte)(255 - Math.Min(255f, _m * factor + black));
            _b = (byte)(255 - Math.Min(255f, _y * factor + black));
            _gs = (float)(1 - Math.Min(1.0, 0.3f * _c + 0.59f * _m + 0.11 * _y + _k));
        }

        ///<summary>
        /// The gray scale value changed; recalculate other color representations.
        /// </summary>
        void GrayChanged()
        {
            _cs = ColorSpace.GrayScale;
            _r = (byte)(_gs * 255);
            _g = (byte)(_gs * 255);
            _b = (byte)(_gs * 255);
            _c = 0;
            _m = 0;
            _y = 0;
            _k = 1 - _gs;
        }

        // Properties

        /// <summary>
        /// Gets or sets the alpha value the specifies the transparency. 
        /// The value is in the range from 1 (opaque) to 0 (completely transparent).
        /// </summary>
        public double A
        {
            get { return _a; }
            set
            {
                if (value < 0)
                    _a = 0;
                else if (value > 1)
                    _a = 1;
                else
                    _a = (float)value;
            }
        }

        /// <summary>
        /// Gets or sets the red value.
        /// </summary>
        public byte R
        {
            get { return _r; }
            set { _r = value; RgbChanged(); }
        }

        /// <summary>
        /// Gets or sets the green value.
        /// </summary>
        public byte G
        {
            get { return _g; }
            set { _g = value; RgbChanged(); }
        }

        /// <summary>
        /// Gets or sets the blue value.
        /// </summary>
        public byte B
        {
            get { return _b; }
            set { _b = value; RgbChanged(); }
        }

        /// <summary>
        /// Gets the RGB part value of the color. Internal helper function.
        /// </summary>
        internal uint Rgb
        {
            get { return ((uint)_r << 16) | ((uint)_g << 8) | _b; }
        }

        /// <summary>
        /// Gets the ARGB part value of the color. Internal helper function.
        /// </summary>
        internal uint Argb
        {
            get { return ((uint)(_a * 255) << 24) | ((uint)_r << 16) | ((uint)_g << 8) | _b; }
        }

        /// <summary>
        /// Gets or sets the cyan value.
        /// </summary>
        public double C
        {
            get { return _c; }
            set
            {
                if (value < 0)
                    _c = 0;
                else if (value > 1)
                    _c = 1;
                else
                    _c = (float)value;
                CmykChanged();
            }
        }

        /// <summary>
        /// Gets or sets the magenta value.
        /// </summary>
        public double M
        {
            get { return _m; }
            set
            {
                if (value < 0)
                    _m = 0;
                else if (value > 1)
                    _m = 1;
                else
                    _m = (float)value;
                CmykChanged();
            }
        }

        /// <summary>
        /// Gets or sets the yellow value.
        /// </summary>
        public double Y
        {
            get { return _y; }
            set
            {
                if (value < 0)
                    _y = 0;
                else if (value > 1)
                    _y = 1;
                else
                    _y = (float)value;
                CmykChanged();
            }
        }

        /// <summary>
        /// Gets or sets the black (or key) value.
        /// </summary>
        public double K
        {
            get { return _k; }
            set
            {
                if (value < 0)
                    _k = 0;
                else if (value > 1)
                    _k = 1;
                else
                    _k = (float)value;
                CmykChanged();
            }
        }

        /// <summary>
        /// Gets or sets the gray scale value.
        /// </summary>
        // ReSharper disable InconsistentNaming
        public double GS
        // ReSharper restore InconsistentNaming
        {
            get { return _gs; }
            set
            {
                if (value < 0)
                    _gs = 0;
                else if (value > 1)
                    _gs = 1;
                else
                    _gs = (float)value;
                GrayChanged();
            }
        }

        /// <summary>
        /// Represents the null color.
        /// </summary>
        public static Color Empty;

        ///<summary>
        /// Special property for XmlSerializer only.
        /// </summary>
        public string RgbCmykG
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture,
                  "{0};{1};{2};{3};{4};{5};{6};{7};{8}", _r, _g, _b, _c, _m, _y, _k, _gs, _a);
            }
            set
            {
                string[] values = value.Split(';');
                _r = byte.Parse(values[0], CultureInfo.InvariantCulture);
                _g = byte.Parse(values[1], CultureInfo.InvariantCulture);
                _b = byte.Parse(values[2], CultureInfo.InvariantCulture);
                _c = float.Parse(values[3], CultureInfo.InvariantCulture);
                _m = float.Parse(values[4], CultureInfo.InvariantCulture);
                _y = float.Parse(values[5], CultureInfo.InvariantCulture);
                _k = float.Parse(values[6], CultureInfo.InvariantCulture);
                _gs = float.Parse(values[7], CultureInfo.InvariantCulture);
                _a = float.Parse(values[8], CultureInfo.InvariantCulture);
            }
        }

        static void CheckByte(int val, string name)
        {
            if (val < 0 || val > 0xFF)
                throw new ArgumentException(name, nameof(name));
        }

        private static string DuplicateTriples(string trip)
        {
            if (trip.Length == 3)
            {
                return new string(new char[] { trip[0], trip[0], trip[1], trip[1], trip[2], trip[2] });
            }
            return trip;
        }

        private static int ToInt(string color)
        {
            var first = ToInt(color[0]);
            var second = ToInt(color[1]);
            return first * 16 + second;
        }

        private static int ToInt(char c)
        {
            if (c >= '0' && c <= '9')
            {
                return c - '0';
            }
            else if (c >= 'A' && c <= 'F')
            {
                return c - 'A';
            }
            else if (c >= 'a' && c <= 'f')
            {
                return c - 'a';
            }
            else
            {
                throw new ArgumentException("Not a hex character", nameof(c));
            }
        }

        ColorSpace _cs;

        float _a;  // alpha

        byte _r;   // \
        byte _g;   // |--- RGB
        byte _b;   // /

        float _c;  // \
        float _m;  // |--- CMYK
        float _y;  // |
        float _k;  // /

        float _gs; // >--- gray scale
    }
}
