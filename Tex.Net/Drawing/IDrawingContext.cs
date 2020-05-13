using Tex.Net.Layout;
using Tex.Net.Text;

namespace Tex.Net.Drawing
{
    public interface IDrawingContext
    {
        Position Offset { get; set; }
        void DrawText(TextRun run, Color color);
        void DrawImage(Image image, Rectangle rectangle);
    }
}
