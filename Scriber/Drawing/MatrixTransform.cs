using System.Numerics;

namespace Scriber.Drawing
{
    public class MatrixTransform : Transform
    {
        public Matrix3x2 Matrix { get; set; }

        public override Matrix3x2 CalculateMatrix()
        {
            return Matrix;
        }
    }
}
