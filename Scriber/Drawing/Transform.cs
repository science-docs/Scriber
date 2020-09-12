using System.Numerics;

namespace Scriber.Drawing
{
    public abstract class Transform
    {
        public static Transform Identity { get; } = new MatrixTransform()
        {
            Matrix = Matrix3x2.Identity
        };

        public abstract Matrix3x2 CalculateMatrix();
    }
}
