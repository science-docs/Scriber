using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Tex.Net.Layout
{
    public enum UnitType
    {
        Point,
        Inch,
        Millimeter,
        Centimeter,
        Presentation
    }

    [DebuggerDisplay("{DebuggerDisplay}")]
    public struct Unit : IFormattable
    {
        internal const double PointFactor = 1;
        internal const double InchFactor = 72;
        internal const double MillimeterFactor = 72 / 25.4;
        internal const double CentimeterFactor = 72 / 2.54;
        internal const double PresentationFactor = 72 / 96.0;

        internal const double PointFactorWpf = 96 / 72.0;
        internal const double InchFactorWpf = 96;
        internal const double MillimeterFactorWpf = 96 / 25.4;
        internal const double CentimeterFactorWpf = 96 / 2.54;
        internal const double PresentationFactorWpf = 1;

        /// <summary>
        /// Initializes a new instance of the XUnit class with type set to point.
        /// </summary>
        public Unit(double point)
        {
            _value = point;
            _type = UnitType.Point;
        }

        /// <summary>
        /// Initializes a new instance of the XUnit class.
        /// </summary>
        public Unit(double value, UnitType type)
        {
            if (!Enum.IsDefined(typeof(UnitType), type))
                throw new System.ComponentModel.InvalidEnumArgumentException("type");
            _value = value;
            _type = type;
        }

        /// <summary>
        /// Gets the raw value of the object without any conversion.
        /// To determine the XGraphicsUnit use property <code>Type</code>.
        /// To get the value in point use the implicit conversion to double.
        /// </summary>
        public double Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Gets the unit of measure.
        /// </summary>
        public UnitType Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Gets or sets the value in point.
        /// </summary>
        public double Point
        {
            get
            {
                return _type switch
                {
                    UnitType.Point => _value,
                    UnitType.Inch => _value * InchFactor,
                    UnitType.Millimeter => _value * MillimeterFactor,
                    UnitType.Centimeter => _value * CentimeterFactor,
                    UnitType.Presentation => _value * PresentationFactor,
                    _ => throw new InvalidCastException(),
                };
            }
            set
            {
                _value = value;
                _type = UnitType.Point;
            }
        }

        /// <summary>
        /// Gets or sets the value in inch.
        /// </summary>
        public double Inch
        {
            get
            {
                return _type switch
                {
                    UnitType.Point => _value / 72,
                    UnitType.Inch => _value,
                    UnitType.Millimeter => _value / 25.4,
                    UnitType.Centimeter => _value / 2.54,
                    UnitType.Presentation => _value / 96,
                    _ => throw new InvalidCastException(),
                };
            }
            set
            {
                _value = value;
                _type = UnitType.Inch;
            }
        }

        /// <summary>
        /// Gets or sets the value in millimeter.
        /// </summary>
        public double Millimeter
        {
            get
            {
                return _type switch
                {
                    UnitType.Point => _value * 25.4 / 72,
                    UnitType.Inch => _value * 25.4,
                    UnitType.Millimeter => _value,
                    UnitType.Centimeter => _value * 10,
                    UnitType.Presentation => _value * 25.4 / 96,
                    _ => throw new InvalidCastException(),
                };
            }
            set
            {
                _value = value;
                _type = UnitType.Millimeter;
            }
        }

        /// <summary>
        /// Gets or sets the value in centimeter.
        /// </summary>
        public double Centimeter
        {
            get
            {
                return _type switch
                {
                    UnitType.Point => _value * 2.54 / 72,
                    UnitType.Inch => _value * 2.54,
                    UnitType.Millimeter => _value / 10,
                    UnitType.Centimeter => _value,
                    UnitType.Presentation => _value * 2.54 / 96,
                    _ => throw new InvalidCastException(),
                };
            }
            set
            {
                _value = value;
                _type = UnitType.Centimeter;
            }
        }

        /// <summary>
        /// Gets or sets the value in presentation units (1/96 inch).
        /// </summary>
        public double Presentation
        {
            get
            {
                return _type switch
                {
                    UnitType.Point => _value * 96 / 72,
                    UnitType.Inch => _value * 96,
                    UnitType.Millimeter => _value * 96 / 25.4,
                    UnitType.Centimeter => _value * 96 / 2.54,
                    UnitType.Presentation => _value,
                    _ => throw new InvalidCastException(),
                };
            }
            set
            {
                _value = value;
                _type = UnitType.Point;
            }
        }

        /// <summary>
        /// Returns the object as string using the format information.
        /// The unit of measure is appended to the end of the string.
        /// </summary>
        public string ToString(IFormatProvider formatProvider)
        {
            string valuestring = _value.ToString(formatProvider) + GetSuffix();
            return valuestring;
        }

        /// <summary>
        /// Returns the object as string using the specified format and format information.
        /// The unit of measure is appended to the end of the string.
        /// </summary>
        string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
        {
            string valuestring = _value.ToString(format, formatProvider) + GetSuffix();
            return valuestring;
        }

        /// <summary>
        /// Returns the object as string. The unit of measure is appended to the end of the string.
        /// </summary>
        public override string ToString()
        {
            string valuestring = _value.ToString(CultureInfo.InvariantCulture) + GetSuffix();
            return valuestring;
        }

        /// <summary>
        /// Returns the unit of measure of the object as a string like 'pt', 'cm', or 'in'.
        /// </summary>
        string GetSuffix()
        {
            return _type switch
            {
                UnitType.Point => "pt",
                UnitType.Inch => "in",
                UnitType.Millimeter => "mm",
                UnitType.Centimeter => "cm",
                UnitType.Presentation => "pu",
                _ => throw new InvalidCastException(),
            };
        }

        /// <summary>
        /// Returns an XUnit object. Sets type to point.
        /// </summary>
        public static Unit FromPoint(double value)
        {
            Unit unit;
            unit._value = value;
            unit._type = UnitType.Point;
            return unit;
        }

        /// <summary>
        /// Returns an XUnit object. Sets type to inch.
        /// </summary>
        public static Unit FromInch(double value)
        {
            Unit unit;
            unit._value = value;
            unit._type = UnitType.Inch;
            return unit;
        }

        /// <summary>
        /// Returns an XUnit object. Sets type to millimeters.
        /// </summary>
        public static Unit FromMillimeter(double value)
        {
            Unit unit;
            unit._value = value;
            unit._type = UnitType.Millimeter;
            return unit;
        }

        /// <summary>
        /// Returns an XUnit object. Sets type to centimeters.
        /// </summary>
        public static Unit FromCentimeter(double value)
        {
            Unit unit;
            unit._value = value;
            unit._type = UnitType.Centimeter;
            return unit;
        }

        /// <summary>
        /// Returns an XUnit object. Sets type to Presentation.
        /// </summary>
        public static Unit FromPresentation(double value)
        {
            Unit unit;
            unit._value = value;
            unit._type = UnitType.Presentation;
            return unit;
        }

        /// <summary>
        /// Memberwise comparison. To compare by value, 
        /// use code like Math.Abs(a.Pt - b.Pt) &lt; 1e-5.
        /// </summary>
        public static bool operator ==(Unit value1, Unit value2)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return value1._type == value2._type && value1._value == value2._value;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>
        /// Memberwise comparison. To compare by value, 
        /// use code like Math.Abs(a.Pt - b.Pt) &lt; 1e-5.
        /// </summary>
        public static bool operator !=(Unit value1, Unit value2)
        {
            return !(value1 == value2);
        }

        /// <summary>
        /// Calls base class Equals.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is Unit)
                return this == (Unit)obj;
            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return _value.GetHashCode() ^ _type.GetHashCode();
        }

        /// <summary>
        /// Converts an existing object from one unit into another unit type.
        /// </summary>
        public void ConvertType(UnitType type)
        {
            if (_type == type)
                return;

            switch (type)
            {
                case UnitType.Point:
                    _value = Point;
                    _type = UnitType.Point;
                    break;

                case UnitType.Inch:
                    _value = Inch;
                    _type = UnitType.Inch;
                    break;

                case UnitType.Centimeter:
                    _value = Centimeter;
                    _type = UnitType.Centimeter;
                    break;

                case UnitType.Millimeter:
                    _value = Millimeter;
                    _type = UnitType.Millimeter;
                    break;

                case UnitType.Presentation:
                    _value = Presentation;
                    _type = UnitType.Presentation;
                    break;

                default:
                    throw new ArgumentException("Unknown unit type: '" + type + "'");
            }
        }

        /// <summary>
        /// Represents a unit with all values zero.
        /// </summary>
        public static readonly Unit Zero = new Unit();

        double _value;
        UnitType _type;

        /// <summary>
        /// Gets the DebuggerDisplayAttribute text.
        /// </summary>
        /// <value>The debugger display.</value>
        string DebuggerDisplay
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "unit=({0:0000} {1})", _value, GetSuffix());
            }
        }
    }
}
