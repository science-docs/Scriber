using Scriber.Drawing.Shapes;
using Scriber.Layout;
using Scriber.Text;

namespace Scriber.Drawing
{
    public interface IDrawingContext
    {
        Position Offset { get; set; }
        void DrawText(TextRun run, Color color);
        void DrawPath(Path path, Color fill, Color stroke, double strokeWidth);
        void DrawImage(Image image, Rectangle rectangle);
        void AddLink(Rectangle rectangle, int targetPage);
        void PushTransform(Transform transform);
        void PopTransform();
    }
}
