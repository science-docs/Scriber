using PdfSharpCore.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tex.Net.Text;

namespace Tex.Net.Layout
{
    public static class LineNodeTransformer
    {
        public static List<LineNode> Create(string text, Font font, double size)
        {
            var spaceWidth = GetWidth(" ");
            //var hyphenWidth = GetWidth("-");
            var stretch = Math.Max(0, spaceWidth / 2);
            var shrink = Math.Max(0, spaceWidth / 3);

            var chunks = Chunkenize(text).ToArray();
            var nodes = new List<LineNode>();

            for (int i = 0; i < chunks.Length; i++)
            {
                if (i > 0)
                {
                    var glue = LineNode.Glue(spaceWidth, stretch, shrink);
                    nodes.Add(glue);
                }

                var box = LineNode.Box(GetWidth(chunks[i]), chunks[i]);
                box.Font = font;
                box.Size = size;
                nodes.Add(box);
            }

            return nodes;

            double GetWidth(string value)
            {
                return font.GetWidth(value);
            }
        }

        private static IEnumerable<string> Chunkenize(string fullText)
        {
            if (string.IsNullOrWhiteSpace(fullText))
            {
                yield break;
            }

            int last = 0;
            for (int i = 1; i < fullText.Length - 1; i++)
            {
                var c = fullText[i];
                if (char.IsWhiteSpace(c))
                {
                    if (i > last - 1)
                    {
                        yield return fullText[last..i];
                    }
                    last = i + 1;
                }
            }
            yield return fullText.Substring(last);
        }
    }
}
