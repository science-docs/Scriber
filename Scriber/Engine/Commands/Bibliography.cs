using Scriber.Autocomplete;
using Scriber.Bibliography;
using Scriber.Bibliography.BibTex.Language;
using Scriber.Bibliography.Styling;
using Scriber.Bibliography.Styling.Formatting;
using Scriber.Bibliography.Styling.Specification;
using Scriber.Layout.Document;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Engine.Commands
{
    [Package("Citation")]
    public static class Bibliography
    {
        [Command("BibliographyStyle")]
        public static void SetBibliographyStyle(CompilerState state, [Argument(Name = "style", ProposalProvider = typeof(BibliographyStyleFileProposalProvider))] Argument<string> style)
        {
            try
            {
                var bytes = state.Context.ResourceSet.RelativeResource(style.Value).GetContent();
                using var sourceStream = new System.IO.MemoryStream(bytes);
                var styleFile = File.Load<StyleFile>(sourceStream);
                state.Document.Citations = new Citations(new Processor(styleFile, LocaleFile.Defaults), state.Document.Locale.File);
            }
            catch (Exception ex)
            {
                throw new CompilerException(style.Source, $"Could not find resource '{style.Value}'", ex);
            }
        }

        [Command("Bibliography")]
        public static void AddBibliography(CompilerState state, [Argument(Name = "bibliography", ProposalProvider = typeof(BibliographyFileProposalProvider))] Argument<string> bibliographyPath)
        {
            if (state.Document.Citations == null)
            {
                return;
            }

            try
            {
                var text = state.Context.ResourceSet.RelativeResource(bibliographyPath.Value).GetContentAsString();
                var bibEntries = BibParser.Parse(text, out var _);
                state.Document.Citations.AddRange(bibEntries.Select(e => e.ToCitation()));
            }
            catch (Exception ex)
            {
                throw new CompilerException(bibliographyPath.Source, $"Could not find resource '{bibliographyPath.Value}'", ex);
            }
        }

        [Command("Cite")]
        public static IEnumerable<Leaf> Cite(CompilerState state, [Argument(Name = "citation", ProposalProvider = typeof(CitationProposalProvider))] Argument<string> citationKey)
        {
            if (state.Document.Citations == null)
            {
                throw new CompilerException(null, "Cannot cite without citations loaded.");
            }

            if (!state.Document.Citations.ContainsKey(citationKey.Value))
            {
                throw new CompilerException(citationKey.Source, $"No citation with key '{citationKey.Value}' exists.");
            }

            var cited = state.Document.Citations.Cite(citationKey.Value);

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
                var paragraph = new Paragraph
                {
                    Margin = new Layout.Thickness(4, 0)
                };

                var leaves = ToLeaves(bibEntry);

                paragraph.Leaves.AddRange(leaves);
                list.Add(paragraph);
            }

            return list;
        }

        private static IEnumerable<Leaf> ToLeaves(Run run)
        {
            foreach (var textRun in run.Flatten())
            {
                yield return ToLeaf(textRun);
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
