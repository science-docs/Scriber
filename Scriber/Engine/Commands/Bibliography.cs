using Scriber.Bibliography;
using Scriber.Bibliography.BibTex.Language;
using Scriber.Bibliography.Styling;
using Scriber.Bibliography.Styling.Formatting;
using Scriber.Bibliography.Styling.Specification;
using Scriber.Layout.Document;
using Scriber.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Engine.Commands
{
    [Package("Citation")]
    public static class Bibliography
    {
        [Command("BibliographyStyle")]
        public static void SetBibliographyStyle(CompilerState state, string source)
        {
            var styleFile = File.Load<StyleFile>(source);
            state.Document.Citations = new Citations(new Processor(styleFile, LocaleFile.Defaults), state.Document.Locale.File);
        }

        [Command("Bibliography")]
        public static void AddBibliography(CompilerState state, string source)
        {
            if (state.Document.Citations == null)
            {
                return;
            }

            if (source.EndsWith(".bib"))
            {
                var bibEntries = BibParser.Parse(System.IO.File.ReadAllText(source), out var _);
                state.Document.Citations.AddRange(bibEntries.Select(e => e.ToCitation()));
            }
        }

        [Command("Cite")]
        public static IEnumerable<Leaf> Cite(CompilerState state, string citationKey)
        {
            if (state.Document.Citations == null)
            {
                return Array.Empty<Leaf>();
            }

            var cited = state.Document.Citations.Cite(citationKey);

            if (cited != null)
            {
                return ToLeaves(cited);
            }
            else
            {
                return Array.Empty<Leaf>();
            }
        }

        [Command("PrintBibliography")]
        public static List<Paragraph> PrintBibliography(CompilerState state)
        {
            if (state.Document.Citations == null)
            {
                return new List<Paragraph>();
            }

            var bibliography = state.Document.Citations.Bibliography();

            var list = new List<Paragraph>();
            foreach (var bibEntry in bibliography)
            {
                var paragraph = new Paragraph();

                var leaves = ToLeaves(bibEntry);

                paragraph.Leaves.AddRange(leaves);
                list.Add(paragraph);
            }

            return list;
        }

        private static IEnumerable<Leaf> ToLeaves(Run run)
        {
            if (run is TextRun textRun)
            {
                yield return ToLeaf(textRun);
            }
            else if (run is ComposedRun composedRun)
            {
                foreach (var child in composedRun.Children)
                {
                    foreach (var leaf in ToLeaves(child))
                    {
                        yield return leaf;
                    }
                }
            }
        }

        private static Leaf ToLeaf(TextRun run)
        {
            var textLeaf = new TextLeaf(run.Text);

            if (run.FontStyle == Scriber.Bibliography.Styling.Formatting.FontStyle.Italic)
            {
                textLeaf.FontWeight = Text.FontWeight.Italic;
            }
            if (run.FontWeight == Scriber.Bibliography.Styling.Formatting.FontWeight.Bold)
            {
                textLeaf.FontWeight = Text.FontWeight.Bold;
            }

            textLeaf.FontStyle = run.VerticalAlign switch
            {
                Scriber.Bibliography.Styling.Formatting.VerticalAlign.Subscript => Text.FontStyle.Subscript,
                Scriber.Bibliography.Styling.Formatting.VerticalAlign.Superscript => Text.FontStyle.Superscript,
                _ => Text.FontStyle.Normal
            };

            return textLeaf;
        }
    }
}
