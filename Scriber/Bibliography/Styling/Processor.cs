using Scriber.Bibliography.Styling.Formatting;
using Scriber.Bibliography.Styling.Specification;
using Scriber.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Bibliography.Styling
{
    public class Processor
    {
        public StyleFile Style { get; }
        public IReadOnlyCollection<LocaleFile> Locales { get; }

        public Processor(StyleFile style, IReadOnlyCollection<LocaleFile> locales)
        {
            Style = style;
            Locales = locales;
        }

        public Run Cite(Culture culture, IEnumerable<Citation> citations)
        {
            return Cite(GetLocale(culture), citations);
        }

        public Run Cite(LocaleFile locale, IEnumerable<Citation> citations)
        {
            Interpreter interpreter = new Interpreter(Style, locale)
            {
                CurrentEntry = Style.Citation
            };
            if (Style.Citation?.Sort != null)
            {
                citations = Style.Citation.Sort.Sort(interpreter, citations);
            }
            
            var runs = new List<Run>();
            var layout = Style.Citation!.Layout!;

            foreach (var citation in citations)
            {
                interpreter.Citation = citation;
                layout.Evaluate(interpreter);
                runs.Add(interpreter.Run);
                interpreter.Clear();
            }

            var multiRun = new List<Run>();
            var delimiter = interpreter.Create(layout.Delimiter, layout);
            var composedRuns = runs.Where(e => !e.IsEmpty).Cast<ComposedRun>().ToArray();

            for (int i = 0; i < composedRuns.Length; i++)
            {
                multiRun.AddRange(composedRuns[i].Children);
                if (delimiter != null && i < composedRuns.Length - 1)
                {
                    multiRun.Add(delimiter);
                }
            }

            return new ComposedRun(string.Empty, multiRun, false);
        }

        public IEnumerable<Run> Bibliography(Culture culture, IEnumerable<Citation> citations)
        {
            return Bibliography(GetLocale(culture), citations);
        }

        public IEnumerable<Run> Bibliography(LocaleFile locale, IEnumerable<Citation> citations)
        {
            Interpreter interpreter = new Interpreter(Style, locale)
            {
                CurrentEntry = Style.Bibliography
            };
            var runs = new List<Run>();

            foreach (var citation in citations)
            {
                interpreter.Citation = citation;
                Style.Bibliography!.Layout!.Evaluate(interpreter);
                runs.Add(interpreter.Run);
                interpreter.Clear();
            }

            return runs;
        }

        private LocaleFile GetLocale(Culture culture)
        {
            return Locales.FirstOrDefault(e => e.XmlLang == culture.Language || e.XmlLang == culture.ToString()) ?? throw new NotImplementedException();
        }
    }
}
