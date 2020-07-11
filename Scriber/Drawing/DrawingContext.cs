using Scriber.Layout;
using Scriber.Text;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Scriber.Drawing
{
    public abstract class DrawingContext : IDrawingContext
    {
        public virtual Position Offset { get; set; }

        private readonly Stack<Matrix3x2> stack = new Stack<Matrix3x2>();
        private Matrix3x2? cache;

        public abstract void AddLink(Rectangle rectangle, int targetPage);
        public abstract void DrawImage(Image image, Rectangle rectangle);
        public abstract void DrawText(TextRun run, Color color);

        protected void ClearTransform()
        {
            cache = null;
            stack.Clear();
        }

        public virtual void PopTransform()
        {
            cache = null;
            stack.Pop();
        }

        public virtual void PushTransform(Transform transform)
        {
            cache = null;
            stack.Push(transform.CalculateMatrix());
        }

        protected Matrix3x2 GetTransform()
        {
            if (!cache.HasValue)
            {
                var identity = Matrix3x2.Identity;
                foreach (var matrix in stack.Reverse())
                {
                    identity *= matrix;
                }
                cache = identity;
            }

            return cache.Value;
        }
    }
}
