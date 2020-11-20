using System;
using System.Numerics;

namespace Scriber.Layout
{
    public struct Position
    {
        public static readonly Position Zero = new Position(0, 0);

        public double X;
        public double Y;

        public Position(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Position Transform(Matrix3x2 matrix)
        {
            var vector = new Vector2((float)X, (float)Y);
            vector = Vector2.Transform(vector, matrix);
            return new Position(vector.X, vector.Y);
        }


        public override bool Equals(object? obj)
        {
            if (obj is Position pos)
            {
                return X == pos.X && Y == pos.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"X: {X:N4} Y: {Y:N4}";
        }

        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }

        public static Position operator +(Position left, Position right)
        {
            return new Position(left.X + right.X, left.Y + right.Y);
        }

        public static Position operator -(Position left, Position right)
        {
            return new Position(left.X - right.X, left.Y - right.Y);
        }

        public static Position operator +(Position left, Size right)
        {
            return new Position(left.X + right.Width, left.Y + right.Height);
        }

        public static Position operator -(Position left, Size right)
        {
            return new Position(left.X - right.Width, left.Y - right.Height);
        }
    }
}
