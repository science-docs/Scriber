using Scriber.Layout;
using System.Numerics;

namespace Scriber.Drawing
{
    public class TranslateTransform : Transform
    {
        public Position Position { get; set; }

        public TranslateTransform()
        {

        }

        public TranslateTransform(Position position)
        {
            Position = position;
        }

        public override Matrix3x2 CalculateMatrix()
        {
            return Matrix3x2.CreateTranslation((float)Position.X, (float)Position.Y);
        }
    }
}
