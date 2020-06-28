using Scriber.Bibliography.Styling.Specification;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Bibliography.Styling.Renderer
{
    public static class DateRenderer
    {
        public static void RenderNonLocalizedDate(Interpreter interpreter, IFormatting? element, IVariable? value, string? delimiter, DatePrecision precision, IEnumerable<DatePartElement> dateParts)
        {
            if (dateParts == null)
            {
                return;
            }

            RenderDate(interpreter, element, value, delimiter, precision, dateParts.ToArray());
        }

        public static void RenderLocalizedDate(Interpreter interpreter, IFormatting? element, IVariable? value, string? delimiter, DateFormat dateFormat, DatePrecision precision, IEnumerable<DatePartElement> dateParts)
        {
            var parts = new List<DatePartElement>();

            var locale = interpreter.LocaleFile;

            var localeDate = locale.Dates!.FirstOrDefault(e => e.Format == dateFormat);

            parts.AddRange(localeDate.DateParts);

            if (dateParts != null)
            {
                foreach (var part in dateParts)
                {
                    Replace(parts, part.Name, part);
                }
            }

            RenderDate(interpreter, element, value, delimiter, precision, parts.ToArray());
        }

        private static void Replace(List<DatePartElement> parts, DatePartName name, DatePartElement part)
        {
            for (int i = 0; i < parts.Count; i++)
            {
                if (parts[i].Name == name)
                {
                    parts[i] = part;
                }
            }
        }


        public static void RenderDate(Interpreter interpreter, IFormatting? element, IVariable? value, string? delimiter, DatePrecision precision, DatePartElement[] dateParts)
        {
            if (!(value is IDateVariable date) || dateParts == null)
            {
                return;
            }

            int precisionValue = (int)precision;

            var available = new List<DatePartName>()
                {
                    DatePartName.Year
                };
            if (date.MonthFrom.HasValue && date.MonthTo.HasValue && dateParts.Any(x => x.Name == DatePartName.Month) && precisionValue < 2)
            {
                // add month
                available.Add(DatePartName.Month);

                // day?
                if (date.DayFrom.HasValue && date.DayTo.HasValue && dateParts.Any(x => x.Name == DatePartName.Day) && precisionValue == 0)
                {
                    available.Add(DatePartName.Day);
                }
            }

            // find differing date parts
            DatePartName[] differing = Array.Empty<DatePartName>();
            if (date.YearFrom != date.YearTo)
            {
                // year, month, day
                differing = available.ToArray();
            }
            else if (date.MonthFrom.HasValue && date.MonthTo.HasValue && date.MonthFrom.Value != date.MonthTo.Value)
            {
                // month, day
                differing = available
                    .Where(x => x != DatePartName.Year)
                    .ToArray();
            }
            else if (date.DayFrom.HasValue && date.DayTo.HasValue && date.DayFrom.Value != date.DayTo.Value)
            {
                // day
                differing = available
                    .Where(x => x == DatePartName.Day)
                    .ToArray();
            }

            // range?
            if (differing.Length == 0)
            {
                // render single date
                RenderDateParts(interpreter, dateParts, date.YearFrom, date.SeasonFrom, date.MonthFrom, date.DayFrom, delimiter, true, true);
            }
            else
            {
                // render from
                var delimiterIndex = Enumerable.Range(1, dateParts.Length)
                    .Select(i => dateParts.Take(i).Select(x => x.Name).ToArray())
                    .First(a => differing.All(x => a.Contains(x)))
                    .Length;
                var fromParts = dateParts
                    .Take(delimiterIndex)
                    .ToArray();

                RenderDateParts(interpreter, fromParts, date.YearFrom, date.SeasonFrom, date.MonthFrom, date.DayFrom, delimiter, true, false);

                var lastFromPart = fromParts[^1];

                interpreter.Push(lastFromPart.RangeDelimiter ?? "–", element);

                // render to
                var toParts = dateParts
                    .Where(x => differing.Contains(x.Name) || !fromParts.Contains(x))
                    .ToArray();
                RenderDateParts(interpreter, toParts, date.YearTo, date.SeasonTo, date.MonthTo, date.DayTo, delimiter, true, false);
            }
        }

        public static void RenderDateParts(Interpreter interpreter,  DatePartElement[] elements, int year, Season? season, int? month, int? day, string? delimiter, bool renderFirstPrefix, bool renderLastSuffix)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                var element = elements[i];
                var renderPrefix = i > 0 || renderFirstPrefix;
                var renderSuffix = i < elements.Length - 1 || renderLastSuffix;

                RenderDatePart(interpreter, element, year, season, month, day, renderPrefix, renderSuffix);

                if (delimiter != null && i < elements.Length - 1)
                {
                    interpreter.Push(delimiter, element);
                }
            }
        }

        public static void RenderDatePart(Interpreter interpreter, DatePartElement element, int year, Season? season, int? month, int? day, bool renderPrefix, bool renderSuffix)
        {
            // init
            string? text = null;

            // per name
            switch (element.Name)
            {
                case DatePartName.Year:
                    // year
                    if (year != 0)
                    {
                        text = element.Format switch
                        {
                            DatePartFormat.Long => string.Format("{0}{1}", Math.Abs(year), (year < 0 ? interpreter.Locale.GetTerm(TermName.Bc, TermFormat.Long) : (year < 1000 ? interpreter.Locale.GetTerm(TermName.Ad, TermFormat.Long) : null))),
                            DatePartFormat.Short => (year % 100).ToString("00"),
                            _ => throw new NotSupportedException(),
                        };
                    }
                    break;
                case DatePartName.Month:
                    if (month.HasValue && month.Value >= 1 && month.Value <= 12)
                    {
                        // month
                        text = element.Format switch
                        {
                            DatePartFormat.Numeric => month.Value.ToString(),
                            DatePartFormat.NumericLeadingZeros => month.Value.ToString("00"),
                            DatePartFormat.Long => interpreter.Locale.GetTerm(GetTermName(month.Value), TermFormat.Long),
                            DatePartFormat.Short => interpreter.Locale.GetTerm(GetTermName(month.Value), TermFormat.Short),
                            DatePartFormat.Ordinal => throw new NotSupportedException(),
                            _ => throw new NotSupportedException(),
                        };
                    }
                    else if (season.HasValue)
                    {
                        // season
                        text = season.Value switch
                        {
                            Season.Spring => interpreter.Locale.GetTerm(TermName.Season01, TermFormat.Long),
                            Season.Summer => interpreter.Locale.GetTerm(TermName.Season02, TermFormat.Long),
                            Season.Autumn => interpreter.Locale.GetTerm(TermName.Season03, TermFormat.Long),
                            Season.Winter => interpreter.Locale.GetTerm(TermName.Season04, TermFormat.Long),
                            _ => throw new NotSupportedException(),
                        };
                    }
                    break;
                case DatePartName.Day:
                    // day
                    if (day.HasValue && day.Value >= 1 && day.Value <= 31)
                    {
                        switch (element.Format)
                        {
                            case DatePartFormat.Numeric:
                                text = day.Value.ToString();
                                break;
                            case DatePartFormat.NumericLeadingZeros:
                                text = day.Value.ToString("00");
                                break;
                            case DatePartFormat.Ordinal:
                                // TODO: this
                                //if (interpreter.Locale.LimitDayOrdinalsToDay1 && day.Value > 1)
                                //{
                                //    text = day.Value.ToString();
                                //}
                                //else
                                //{
                                //    // get gender
                                //    var gender = (month.HasValue ? interpreter.Locale.GetTermGender(GetTermName(month.Value)) : (Gender?)null);

                                //    // done
                                //    text = interpreter.Locale.FormatNumberAsOrdinal((uint)day.Value, gender);
                                //}
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                    }
                    break;
            }

            if (text != null)
            {
                if (renderPrefix)
                {
                    interpreter.Push(element.Prefix, element);
                }

                interpreter.Push(text, element);

                if (renderSuffix)
                {
                    interpreter.Push(element.Suffix, element);
                }
            }
        }

        private static TermName GetTermName(int month)
        {
            return month switch
            {
                1 => TermName.Month01,
                2 => TermName.Month02,
                3 => TermName.Month03,
                4 => TermName.Month04,
                5 => TermName.Month05,
                6 => TermName.Month06,
                7 => TermName.Month07,
                8 => TermName.Month08,
                9 => TermName.Month09,
                10 => TermName.Month10,
                11 => TermName.Month11,
                12 => TermName.Month12,
                _ => throw new NotSupportedException(),
            };
        }
    }
}
