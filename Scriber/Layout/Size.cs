using System;

namespace Scriber.Layout
{
    public struct Size
    {
        public static readonly Size Zero = new Size(0, 0);

        public double Width;
        public double Height;

        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Size size)
            {
                return Width == size.Width && Height == size.Height;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Width, Height);
        }

        public override string ToString()
        {
            return $"Width: {Width:N4} Height: {Height:N4}";
        }

        public static bool operator ==(Size left, Size right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Size left, Size right)
        {
            return !(left == right);
        }
    }
}
