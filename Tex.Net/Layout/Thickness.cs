namespace Tex.Net.Layout
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
            return Top.GetHashCode() + Bottom.GetHashCode() * 13 + Left.GetHashCode() * 31 + Right.GetHashCode() * 37;
        }

        public static bool operator ==(Thickness left, Thickness right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Thickness left, Thickness right)
        {
            return !(left == right);
        }
    }
}
