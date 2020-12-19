using Scriber.Bibliography.Styling.Formatting;
using Scriber.Bibliography.Styling.Specification;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Bibliography.Styling.Renderer
{
    public static partial class NameRenderer
    {
        public static void RenderNames(
            Interpreter interpreter,
            IFormatting? formatting,
            string[] variables,
            TermName?[] terms,
            string? subsequentAuthorSubstitute,
            SubsequentAuthorSubstituteRules? subsequentAuthorSubstituteRule,
            NameElement name,
            EtAlElement? etAl,
            LabelElement? label,
            Func<NameElement?, EtAlElement?, LabelElement?, bool>[] substitutes)
        {
            if (!RenderNameGroups(interpreter, formatting, variables, terms, false, subsequentAuthorSubstitute, subsequentAuthorSubstituteRule, name, etAl, label))
            {
                int i = 0;
                while (i < substitutes.Length && !substitutes[i].Invoke(name, etAl, label))
                {
                    i++;
                }
            }
        }

        private static bool RenderNameGroups(
            Interpreter interpreter,
            IFormatting? formatting,
            string[] variables,
            TermName?[] terms,
            bool suppress,
            string? subsequentAuthorSubstitute,
            SubsequentAuthorSubstituteRules? subsequentAuthorSubstituteRule,
            NameElement name,
            EtAlElement? etAl,
            LabelElement? label)
        {
            var groups = new List<NameGroup>();

            for (int i = 0; i < terms.Length; i++)
            {
                var variable = variables[i];
                var names = interpreter.Variable(variable);
                if (names is INamesVariable namesVariable)
                {
                    var group = new NameGroup(variable, terms[i], namesVariable);
                    groups.Add(group);
                }
            }

            if (suppress)
            {
                foreach (var variable in variables)
                {
                    interpreter.SuppressVariable(variable);
                }
            }

            var editors = groups.SingleOrDefault(x => x.Term.HasValue && x.Term.Value == TermName.Editor);
            var translators = groups.SingleOrDefault(x => x.Term.HasValue && x.Term.Value == TermName.Translator);
            if (editors != null && translators != null)
            {
                // identical?
                if (editors.Names.Select(x => x.ToString()).SequenceEqual(translators.Names.Select(x => x.ToString())))
                {
                    // insert
                    groups.Insert(groups.IndexOf(editors), new NameGroup(null, TermName.EditorTranslator, editors.Names));
                    groups.Remove(editors);
                    groups.Remove(translators);
                }
            }

            // create results
            if (name.Format == NameFormat.Count)
            {
                // count
                var count = groups
                    .Select(x => x.Names.Count >= name.EtAlMin ? Math.Max(name.EtAlUseFirst, interpreter.DisambiguationContext.MinAddNames) : x.Names.Count)
                    .Sum();

                interpreter.Push(count > 0 ? count.ToString() : string.Empty, formatting);
                return true;
            }
            else
            {
                bool result = false;

                for (int i = 0; i < groups.Count; i++)
                {
                    var group = groups[i];

                    NameGroup? previousGroup = null;

                    // subsequent author substitution?
                    if (subsequentAuthorSubstituteRule.HasValue)
                    {
                        // todo
                        // find match from previous entry in bibliography
                        previousGroup = interpreter.Previous == null || interpreter.Previous.FirstBibliographyNameGroups.Count <= i ? null : interpreter.Previous.FirstBibliographyNameGroups[i];
                    }

                    // render name group
                    var groupResult = RenderNameGroup(interpreter, formatting, group, previousGroup, subsequentAuthorSubstitute, subsequentAuthorSubstituteRule, name, etAl, label);

                    if (groupResult)
                    {
                        result = true;
                    }
                }
                // set result
                if (subsequentAuthorSubstituteRule.HasValue)
                {
                    interpreter.FirstBibliographyNameGroups = groups.ToArray();
                }

                return result;
            }
        }

        private static bool RenderNameGroup(
            Interpreter interpreter,
            IFormatting? formatting,
            NameGroup group,
            NameGroup? previousGroup,
            string? subsequentAuthorSubstitute,
            SubsequentAuthorSubstituteRules? subsequentAuthorSubstituteRule,
            NameElement name,
            EtAlElement? etAl,
            LabelElement? label)
        {
            var result = false;

            // complete subsequent author substitution?
            var isComplete = subsequentAuthorSubstituteRule.HasValue &&
                previousGroup?.Names != null &&
                group.Names.Count == previousGroup.Names.Count &&
                Enumerable.Range(0, group.Names.Count).All(i => NameGroup.AreNamesEqual(group.Names[i], previousGroup.Names[i]));
            if (isComplete && subsequentAuthorSubstituteRule!.Value == SubsequentAuthorSubstituteRules.CompleteAll)
            {
                // substitute subsequent author
                result = true;
                interpreter.Push(subsequentAuthorSubstitute, formatting);
            }
            else
            {
                // render names list
                var etAlActive = group.Names.Count >= name.EtAlMin;
                var count = etAlActive ? Math.Max(name.EtAlUseFirst, interpreter.DisambiguationContext.MinAddNames) + 1 : group.Names.Count;
                var delta = etAlActive ? 1 : 0;

                // render names
                var parts = group.Names
                    .Where((x, i) => i < count - delta || i == group.Names.Count - 1)
                    .Select((nameItem, index) =>
                    {
                        // invert name?
                        var inverted = false;
                        switch (name.NameAsSortOrder)
                        {
                            case NameSortOptions.None:
                                inverted = false;
                                break;
                            case NameSortOptions.First:
                                inverted = (index == 0);
                                break;
                            case NameSortOptions.All:
                                inverted = true;
                                break;
                        }

                        // subsequent author substitute?
                        bool substitute = false;
                        if (subsequentAuthorSubstituteRule.HasValue &&
                            previousGroup?.Names != null &&
                            index < previousGroup.Names.Count &&
                            NameGroup.AreNamesEqual(nameItem, previousGroup.Names[index]))
                        {
                            switch (subsequentAuthorSubstituteRule.Value)
                            {
                                case SubsequentAuthorSubstituteRules.CompleteEach:
                                    substitute = isComplete;
                                    break;
                                case SubsequentAuthorSubstituteRules.PartialEach:
                                    substitute = true;
                                    break;
                                case SubsequentAuthorSubstituteRules.PartialFirst:
                                    substitute = index == 0;
                                    break;
                            }
                        }

                        // substitute?
                        result = true;

                        // render name
                        return new
                        {
                            Item = nameItem,
                            Substitute = substitute,
                            Inverted = inverted,
                            IsSecondToLast = (index == count - 2),
                            IsLast = (index == count - 1)
                        };
                    })
                    .ToArray();

                bool isFirst = true;
                // create results
                foreach (var part in parts)
                {
                    // name
                    if (part.IsLast && etAlActive && etAl != null && !name.EtAlUseLast)
                    {
                        // add 'et-al' only if any names are rendered (which is not the case when et-al-use-first="0")
                        if (!isFirst)
                        {
                            isFirst = false;
                            interpreter.Push(interpreter.Locale.GetTerm(etAl.Term, TermFormat.Long, false), formatting);
                        }
                    }
                    else
                    {
                        isFirst = false;
                        if (part.Substitute)
                        {
                            // subsequent author substitute
                            result = true;
                            interpreter.Push(subsequentAuthorSubstitute, formatting);
                        }
                        else
                        {
                            // name
                            result = true;
                            RenderName(interpreter, part.Item, part.Inverted, name);
                        }
                    }

                    // delimiter
                    if (part.IsLast)
                    {
                        // no delimiter
                    }
                    else if (part.IsSecondToLast)
                    {
                        isFirst = false;
                        // init
                        var addDelimiter = true;

                        // et al or and?
                        if (etAlActive)
                        {
                            // et al
                            addDelimiter = EvaluateDelimiterBehavior(name.DelimiterPrecedesEtAl, count > 2, part.Inverted);
                        }
                        else if (name.And != And.Delimiter)
                        {
                            // and
                            addDelimiter = EvaluateDelimiterBehavior(name.DelimiterPrecedesLast, count >= 3, part.Inverted);
                        }

                        // add delimiter
                        interpreter.Push(addDelimiter ? interpreter.CurrentEntry!.NameDelimiter ?? interpreter.StyleFile.NameDelimiter : " ", formatting);

                        // and
                        if (!etAlActive)
                        {
                            switch (name.And)
                            {
                                case And.Symbol:
                                    interpreter.Push("& ", formatting);
                                    break;
                                case And.Text:
                                    interpreter.Push(interpreter.Locale.GetTerm(TermName.And, TermFormat.Long, false), formatting);
                                    break;
                            }
                        }

                        // et-al-use-last
                        if (etAlActive && name.EtAlUseLast)
                        {
                            // ellipsis
                            interpreter.Push("… ", formatting);
                        }
                    }
                    else
                    {
                        isFirst = false;
                        // add delimiter
                        interpreter.Push(interpreter.CurrentEntry!.NameDelimiter ?? interpreter.StyleFile.NameDelimiter, formatting);
                    }
                }

                // label
                if (label != null && group.Term.HasValue)
                {
                    // plural?
                    var plural = false;
                    plural = label.Plurilization switch
                    {
                        LabelPluralization.Always => true,
                        LabelPluralization.Contextual => (group.Names.Count != 1),
                        LabelPluralization.Never => false,
                        _ => throw new NotSupportedException(),
                    };

                    // render
                    var text = interpreter.Locale.GetTerm(group.Term.Value, label.Format, plural);

                    if (!string.IsNullOrEmpty(text))
                    {
                        interpreter.Push(label.Prefix, label);
                        interpreter.Push(text, label);
                        interpreter.Push(label.Suffix, label);
                    }
                }
            }

            // done
            return result;
        }
        private static void RenderName(Interpreter interpreter, object name, bool inverted, NameElement nameElement)
        {
            if (name is IPersonalName personalName)
            {
                RenderName(interpreter, personalName, inverted, nameElement);
            }
            else if (name is IInstitutionalName institutionalName)
            {
                interpreter.Push(institutionalName.Name, nameElement);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
        private static void RenderName(Interpreter interpreter, IPersonalName name, bool inverted, NameElement nameElement)
        {
            // init
            var familyPart = GetNamePart(nameElement, 0);
            var givenPart = GetNamePart(nameElement, 1);
            var suffixPart = nameElement;

            // format name parts
            var familyName = interpreter.Create(name.FamilyName, familyPart);
            var nonDroppingParticles = interpreter.Create(name.NonDroppingParticles, familyPart);
            var given = interpreter.Create(InitializeGivenNames(interpreter, name, nameElement.Initialize && (interpreter.DisambiguationContext.AddGivenNameLevel != DisambiguateAddGivenNameLevel.LongAndUninitialized), nameElement.InitializeWith), givenPart);
            var droppingParticles = interpreter.Create(name.DroppingParticles, givenPart);
            var suffix = interpreter.Create(name.PrecedeSuffixByComma && !string.IsNullOrWhiteSpace(name.Suffix) ? string.Format(",{0}", name.Suffix) : name.Suffix, suffixPart);

            // space delimiter
            var space = interpreter.Create(" ", nameElement)!;

            // render parts
            List<Run> runs = new List<Run>();
            string? delimiter;
            switch (interpreter.DisambiguationContext.AddGivenNameLevel != DisambiguateAddGivenNameLevel.None ? NameFormat.Long : interpreter.CurrentEntry!.NameFormat)
            {
                case NameFormat.Long:
                    // inverted?
                    if (inverted)
                    {
                        // demote non dropping particle?
                        if (interpreter.StyleFile.DemoteNonDroppingParticle == DemotingBehavior.DisplayAndSort)
                        {
                            // yes
                            delimiter = interpreter.CurrentEntry!.SortSeparator;

                            runs.AddRange(RenderNamePart(familyPart, space, familyName));
                            runs.AddRange(RenderNamePart(givenPart, space, given, droppingParticles, nonDroppingParticles));
                            runs.AddRange(RenderNamePart(suffixPart, space, suffix));
                        }
                        else
                        {
                            // no
                            delimiter = interpreter.CurrentEntry!.SortSeparator;

                            runs.AddRange(RenderNamePart(familyPart, space, nonDroppingParticles, familyName));
                            runs.AddRange(RenderNamePart(givenPart, space, given, droppingParticles));
                            runs.AddRange(RenderNamePart(suffixPart, space, suffix));
                        }
                    }
                    else
                    {
                        // not inverted
                        delimiter = " ";
                        runs.AddRange(RenderNamePart(givenPart, space, given));
                        runs.AddRange(RenderNamePart(familyPart, space, droppingParticles, nonDroppingParticles, familyName, suffix));
                    }
                    break;
                case NameFormat.Short:
                    // short
                    delimiter = null;
                    runs.AddRange(RenderNamePart(familyPart, space, nonDroppingParticles, familyName));
                    break;
                default:
                    throw new NotSupportedException();
            }

            // apply delimiters
            var delimiterRun = interpreter.Create(delimiter, nameElement);
            if (delimiterRun != null)
            {
                interpreter.PushWithDelimiter(delimiterRun, runs.ToArray());
            }
            else
            {
                interpreter.Push(runs.ToArray());
            }
        }

        private static IFormatting GetNamePart(NameElement name, int index)
        {
            if (name.NameParts == null || name.NameParts.Count <= index)
            {
                return name;
            }
            return name.NameParts[index];
        }

        private static IEnumerable<TextRun> RenderNamePart(IFormatting formatting, TextRun space, params TextRun?[] parts)
        {
            // filter
            TextRun[] filtered = parts
                .Where(part => part != null && part.Text.Length > 0)
                .ToArray()!;

            var results = new List<TextRun>();

            for (int i = 0; i < filtered.Length; i++)
            {
                var item = filtered[i];
                results.Add(item);
                if (i != filtered.Length - 1 && !"'’‘".Contains(item.Text[^1]))
                {
                    // with space
                    results.Add(space);
                }
            }

            // done
            return results;
        }
        private static string InitializeGivenNames(Interpreter interpreter, IPersonalName name, bool initialize, string? initializeWith)
        {
            // processing required?
            if (initializeWith == null || string.IsNullOrEmpty(name.GivenNames) || string.IsNullOrWhiteSpace(name.FamilyName))
            {
                // nope
                return name.GivenNames;
            }

            // split
            var names = name.GivenNames
                .Split(new char[] { ' ', '.' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(givenName =>
                {
                    // initial?
                    if (givenName.Length == 1)
                    {
                        // initial
                        return string.Format("{0}{1}", givenName.ToUpper(), initializeWith);
                    }
                    else
                    {
                        // initialize?
                        if (initialize)
                        {
                            // compound name?
                            var parts = givenName.Split(new char[] { '-', '_', '–' }, StringSplitOptions.RemoveEmptyEntries);

                            // done
                            if (interpreter.StyleFile.InitializeWithHyphen)
                            {
                                return string.Format("{0}{1}", string.Join(string.Format("{0}-", initializeWith.Trim()), parts.Select(x => x.Substring(0, 1).ToUpper())), initializeWith);
                            }
                            else
                            {
                                return string.Join("", parts.Select(x => string.Format("{0}{1}", x.Substring(0, 1).ToUpper(), initializeWith)));
                            }
                        }
                        else
                        {
                            // return full name
                            return string.Format("{0} ", givenName);
                        }
                    }
                })
                .ToArray();

            // done
            return string.Join("", names).Trim();
        }

        private static bool EvaluateDelimiterBehavior(DelimiterBehavior behavior, bool isContext, bool isInverted)
        {
            return behavior switch
            {
                DelimiterBehavior.AfterInvertedName => isInverted,
                DelimiterBehavior.Always => true,
                DelimiterBehavior.Contextual => isContext,
                DelimiterBehavior.Never => false,
                _ => throw new NotSupportedException(),
            };
        }
    }
}
