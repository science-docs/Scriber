using System;

namespace Scriber.Layout
{
    public struct Thickness
    {
        public static readonly Thickness Zero = new Thickness(0);

        public double Top;
        public double Bottom;
        public double Left;
        public double Right;

        public double Height => Top + Bottom;
        public double Width => Left + Right;

        public Thickness(double uniform) : this(uniform, uniform, uniform, uniform)
        {

        }

        public Thickness(double topBottom, double leftRight) : this(topBottom, leftRight, topBottom, leftRight)
        {

        }

        public Thickness(double top, double left, double bottom, double right)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Thickness thickness)
            {
                return Top == thickness.Top && Bottom == thickness.Bottom && Left == thickness.Left && Right == thickness.Right;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Top, Bottom, Left, Right);
        }

        public static bool operator ==(Thickness left, Thickness right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Thickness left, Thickness right)
        {
            return !(left == right);
        }

        public static bool TryParse(string input, out Thickness thickness)
        {
            var match = Unit.ParserRegex.Match(input.Trim());
            thickness = Zero;

            if (!match.Success)
            {
                return false;
            }

            var units = match.Groups["unit"].Captures;

            if (units.Count != 1 && units.Count != 2 && units.Count != 4)
            {
                return false;
            }

            if (units.Count == 1)
            {
                if (!Unit.TryParse(units[0].Value, out var unit))
                {
                    return false;
                }

                thickness = new Thickness(unit.Point);
                return true;
            }
            else if (units.Count == 2)
            {
                if (!Unit.TryParse(units[0].Value, out var topBottom))
                {
                    return false;
                }
                if (!Unit.TryParse(units[1].Value, out var leftRight))
                {
                    return false;
                }

                thickness = new Thickness(topBottom.Point, leftRight.Point);
                return true;
            }
            else if (units.Count == 4)
            {
                if (!Unit.TryParse(units[0].Value, out var top))
                {
                    return false;
                }
                if (!Unit.TryParse(units[1].Value, out var left))
                {
                    return false;
                }
                if (!Unit.TryParse(units[2].Value, out var bottom))
                {
                    return false;
                }
                if (!Unit.TryParse(units[3].Value, out var right))
                {
                    return false;
                }

                thickness = new Thickness(top.Point, left.Point, bottom.Point, right.Point);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
