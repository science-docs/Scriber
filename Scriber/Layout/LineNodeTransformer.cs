using System;
using System.Collections.Generic;
using System.Linq;
using Scriber.Layout.Document;
using Scriber.Text;

namespace Scriber.Layout
{
    public static class LineNodeTransformer
    {
        public static void Clean(List<LineNode> nodes)
        {
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                if (nodes[i].Type == LineNodeType.Glue && nodes[i + 1].Type == LineNodeType.Glue)
                {
                    nodes.RemoveAt(i);
                }
            }
            if (nodes.Count > 0 && nodes[0].Type == LineNodeType.Glue)
            {
                nodes.RemoveAt(0);
            }
            if (nodes.Count > 0 && nodes[^1].Type != LineNodeType.Glue)
            {
                nodes.Add(LineNode.Glue(0, 0, 0));
            }
        }

        public static LineNode GetDefaultGlue(AbstractElement element)
        {
            var font = element.Font ?? throw new ArgumentException("Specified element missing font", nameof(element));
            var size = element.FontSize;
            var spaceWidth = GetWidth(" ");
            var stretch = Math.Max(0, spaceWidth / 2);
            var shrink = Math.Max(0, spaceWidth / 3);

            return LineNode.Glue(spaceWidth, stretch, shrink);

            double GetWidth(string value)
            {
                return font.GetWidth(value, size, element.FontWeight);
            }
        }

        public static List<LineNode> Create<T>(T leaf) where T : Leaf, ITextLeaf
        {
            return Create(leaf, leaf.Content);
        }

        public static List<LineNode> Create(Leaf leaf, string? text)
        {
            if (text == null)
                return new List<LineNode>();

            var font = leaf.Font ?? throw new ArgumentException("Specified element missing font", nameof(leaf));
            var size = FontStyler.ScaleSize(leaf.FontStyle, leaf.FontSize);
            var spaceWidth = GetWidth(" ");
            var hyphenWidth = GetWidth("-");
            var stretch = Math.Max(0, spaceWidth / 2);
            var shrink = Math.Max(0, spaceWidth / 3);

            var chunks = Chunkenize(text).ToArray();
            var nodes = new List<LineNode>();
            Glue(text, out bool start, out bool end);

            Hyphenator? hyph = leaf.Document?.Hyphenator;

            if (start)
            {
                nodes.Add(LineNode.Glue(spaceWidth, stretch, shrink));
            }

            for (int i = 0; i < chunks.Length; i++)
            {
                if (i > 0)
                {
                    var glue = LineNode.Glue(spaceWidth, stretch, shrink);
                    nodes.Add(glue);
                }

                if (hyph != null)
                {
                    var hyphenated = hyph.Hyphenate(chunks[i]);

                    for (int j = 0; j < hyphenated.Length; j++)
                    {
                        var box = LineNode.Box(GetWidth(hyphenated[j]), hyphenated[j]);
                        box.Element = leaf;
                        nodes.Add(box);

                        if (j < hyphenated.Length - 1)
                        {
                            var penalty = LineNode.Penalty(hyphenWidth, 10, true);
                            penalty.Element = leaf;
                            nodes.Add(penalty);
                        }
                    }
                }
                else
                {
                    var box = LineNode.Box(GetWidth(chunks[i]), chunks[i]);
                    box.Element = leaf;
                    nodes.Add(box);
                }

                
            }

            if (end)
            {
                nodes.Add(LineNode.Glue(spaceWidth, stretch, shrink));
            }

            return nodes;

            double GetWidth(string value)
            {
                return font.GetWidth(value, size, leaf.FontWeight);
            }
        }

        private static void Glue(string fullText, out bool start, out bool end)
        {
            if (!string.IsNullOrEmpty(fullText))
            {
                start = char.IsWhiteSpace(fullText[0]);
                end = char.IsWhiteSpace(fullText[^1]);
            }
            else
            {
                start = false;
                end = false;
            }
        }

        private static IEnumerable<string> Chunkenize(string fullText)
        {
            if (string.IsNullOrWhiteSpace(fullText))
            {
                yield break;
            }

            fullText = fullText.Trim();


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
