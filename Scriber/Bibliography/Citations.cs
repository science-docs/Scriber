using Scriber.Bibliography.Styling;
using Scriber.Bibliography.Styling.Formatting;
using Scriber.Bibliography.Styling.Specification;
using System;
using System.Collections.Generic;

namespace Scriber.Bibliography
{
    public class Citations
    {
        public Processor? Processor { get; set; }
        public LocaleFile? Locale { get; set; }

        private readonly Dictionary<string, Citation> citations = new();
        private readonly Dictionary<string, Citation> citedCitations = new();

        public Citations()
        {

        }

        public Citations(Processor? processor, LocaleFile? locale)
        {
            Processor = processor;
            Locale = locale;
        }

        public void Add(Citation citation)
        {
            citations[citation.Key] = citation;
        }

        public void AddRange(IEnumerable<Citation> citations)
        {
            foreach (var citation in citations)
            {
                Add(citation);
            }
        }

        public bool ContainsKey(string key)
        {
            return citations.ContainsKey(key);
        }

        public IEnumerable<Citation> GetCitations()
        {
            return citations.Values;
        }

        public Run Cite(params string[] keys)
        {
            return Cite((IEnumerable<string>)keys);
        }

        public Run Cite(IEnumerable<string> keys)
        {
            if (Processor == null)
            {
                throw new NullReferenceException(nameof(Processor));
            }
            if (Locale == null)
            {
                throw new NullReferenceException(nameof(Locale));
            }

            var list = new List<Citation>();
            foreach (var key in keys)
            {
                if (!citedCitations.TryGetValue(key, out var citation))
                {
                    if (!citations.TryGetValue(key, out citation))
                    {
                        throw new Exception();
                    }
                    else
                    {
                        citedCitations[key] = citation;
                        citation["citation-number"] = new TextVariable(citedCitations.Count.ToString());
                    }
                }

                list.Add(citation);
            }

            return Processor.Cite(Locale, list);
        }

        public IEnumerable<Run> Bibliography()
        {
            if (Processor == null)
            {
                throw new NullReferenceException(nameof(Processor));
            }
            if (Locale == null)
            {
                throw new NullReferenceException(nameof(Locale));
            }

            return Processor.Bibliography(Locale, citedCitations.Values);
        }
    }
}
