using Scriber.Layout;
using Scriber.Text;

namespace Scriber.Drawing
{
    public interface IDrawingContext
    {
        Position Offset { get; set; }
        void DrawText(TextRun run, Color color);
        void DrawImage(Image image, Rectangle rectangle);
    }
}
