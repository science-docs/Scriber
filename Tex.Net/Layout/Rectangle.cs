namespace Tex.Net.Layout
{
    public struct Rectangle
    {
        public double X;
        public double Y;
        public double Width;
        public double Height;

        public Size Size
        {
            get => new Size(Width, Height);
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        public Position Position
        {
            get => new Position(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Rectangle(Position position, Size size) : this(position.X, position.Y, size.Width, size.Height)
        {

        }

        public Rectangle(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public override bool Equals(object obj)
        {
            if (obj is Rectangle rect)
            {
                return X == rect.X && Y == rect.Y && Width == rect.Width && Height == rect.Height;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() * 13 + Width.GetHashCode() * 31 + Height.GetHashCode() * 37;
        }

        public static bool operator ==(Rectangle left, Rectangle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Rectangle left, Rectangle right)
        {
            return !(left == right);
        }
    }
}
