using Scriber.Bibliography.Styling.Formatting;
using Scriber.Bibliography.Styling.Renderer;
using Scriber.Bibliography.Styling.Specification;
using Scriber.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Bibliography.Styling
{
    public class Interpreter
    {
        public StyleFile StyleFile { get; }
        public LocaleFile LocaleFile { get; }
        public Locale Locale { get; }

        public EntryElement? CurrentEntry { get; set; }

        public DisambiguationContext DisambiguationContext { get; } = new DisambiguationContext();

        public NameGroup[] FirstBibliographyNameGroups { get; set; } = Array.Empty<NameGroup>();

        public ComposedRun Run { get; private set; }

        public Interpreter? Previous { get; private set; }

        public Citation? Citation
        {
            get => citation;
            set
            {
                citation = value;
                variables.Clear();
                if (citation != null)
                {
                    citation.CopyTo(variables);
                }
            }
        }

        public IVariable? Variable(string name)
        {
            if (variables.TryGetValue(name, out var variable))
            {
                return variable;
            }
            return null;
        }

        public void SuppressVariable(string name)
        {
            variables.Remove(name);
        }

        private Citation? citation;
        private readonly Dictionary<string, IVariable> variables;

        public Interpreter(StyleFile style, LocaleFile locale)
        {
            StyleFile = style;
            LocaleFile = locale;
            Locale = new Locale(locale);
            variables = new Dictionary<string, IVariable>();
            Run = new ComposedRun(string.Empty, Array.Empty<Run>(), false);
        }

        public void Clear()
        {
            Run = new ComposedRun(string.Empty, Array.Empty<Run>(), false);
        }

        public void Push(params Run[] runs)
        {
            Run.Children.AddRange(runs);
        }

        public void PushWithDelimiter(TextRun? delimiter, params Run[] runs)
        {
            var nonEmptyRuns = runs.Where(e => !e.IsEmpty).ToArray();

            if (nonEmptyRuns.Length == 0)
            {
                return;
            }
            else if (delimiter == null)
            {
                Push(nonEmptyRuns);
                return;
            }

            for (int i = 0; i < nonEmptyRuns.Length - 1; i++)
            {
                Run.Children.Add(nonEmptyRuns[i]);
                Run.Children.Add(delimiter);
            }

            Run.Children.Add(nonEmptyRuns[^1]);
        }

        public void Join(Citation citation, IFormatting? formatting, string? delimiter, params IEvaluated[] runs)
        {
            Join(citation, Create(delimiter, formatting), runs);
        }

        public void Join(Citation citation, params IEvaluated[] runs)
        {
            Join(citation, null, runs);
        }

        public void Join(Citation citation, Run? delimiter, params IEvaluated[] runs)
        {
            bool anyAdded = false;
            for (int i = 0; i < runs.Length; i++)
            {
                var count = Run.Children.Count;
                runs[i].Evaluate(this, citation);
                if (delimiter != null && count < Run.Children.Count)
                {
                    if (anyAdded)
                    {
                        Run.Children.Insert(count, delimiter);
                    }
                    anyAdded = true;
                }
            }
        }

        public TextRun? Create(string? text, IFormatting? format)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            if (format is IStripPeriods stripPeriods && stripPeriods.StripPeriods)
            {
                text = text.Replace(".", string.Empty);
            }

            return new TextRun(text,
                (Formatting.FontStyle)(int)(format?.FontStyle ?? 0),
                (Formatting.FontVariant)(int)(format?.FontVariant ?? 0),
                (Formatting.FontWeight)(int)(format?.FontWeight ?? 0),
                (Formatting.TextDecoration)(int)(format?.TextDecoration ?? 0),
                (Formatting.VerticalAlign)(int)(format?.VerticalAlign ?? 0));
        }

        public void Insert(int index, string? text, IFormatting? format)
        {
            var run = Create(text, format);
            if (run != null && !run.IsEmpty)
            {
                Run.Children.Insert(index, run);
            }
        }

        public void Push(string? text, IFormatting? format)
        {
            var run = Create(text, format);
            if (run != null)
            {
                Push(run);
            }
        }
    }
}
