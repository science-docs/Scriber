using Scriber.Layout;
using Scriber.Layout.Document;
using Scriber.Variables;

namespace Scriber.Engine.Commands
{
    [Package("")]
    public static class Layouts
    {
        [Command("VerticalSpace")]
        public static Box VSpace(Unit vertical)
        {
            return new Box(new Size(0, vertical.Point));
        }

        [Command("Centering")]
        public static CallbackArrangingBlock Centering()
        {
            return new CallbackArrangingBlock(Center);

            static void Center(DocumentElement block)
            {
                if (block.Parent is Panel panel)
                {
                    for (int i = 0; i < panel.Elements.Count; i++)
                    {
                        panel.Elements[i].HorizontalAlignment = HorizontalAlignment.Center;
                    }
                }
            }
        }

        [Command("SetLength")]
        public static void SetLength(CompilerState state, string lengthName, string length)
        {
            var l = double.Parse(length);

            // TODO:
            //state.Document.Variables[DocumentVariables.Length][lengthName].SetValue(l);
        }

        [Command("SetPageNumber")]
        public static CallbackArrangingBlock SetPageNumber(int count)
        {
            return new CallbackArrangingBlock(Set);

            void Set(DocumentElement element)
            {
                element.Document?.PageNumbering.Set(count);
            }
        }

        [Command("Geometry")]
        public static void ChangeGeometry(CompilerState state, Geometry geometry)
        {
            var margin = new Thickness(geometry.Top.Point, geometry.Left.Point, geometry.Bottom.Point, geometry.Right.Point);
            PageVariables.Margin.Set(state.Document, margin);
        }
    }

    public class Geometry
    {
        public Unit Left { get; set; }
        public Unit Top { get; set; }

        public Unit Right { get; set; }

        public Unit Bottom { get; set; }
    }

}
