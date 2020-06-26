using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Scriber.Bibliography.Styling.Specification
{
    /// <summary>
    /// Represents a cs:if or cs:else-if conditional branch inside a cs:choose element.
    /// </summary>
    public class IfElement : ElseElement
    {
        /// <summary>
        /// When set to “true” (the only allowed value), the element content is only rendered if it disambiguates two otherwise
        /// identical citations. This attempt at disambiguation is only made when all other disambiguation methods have failed
        /// to uniquely identify the target source.
        /// </summary>
        [XmlAttribute("disambiguate")]
        public bool Disambiguate
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'disambiguate' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool DisambiguateSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Tests whether the given variable(s) contain numeric content. Content is considered numeric if it solely consists
        /// of numbers. Numbers may have prefixes and suffixes (“D2”, “2b”, “L2d”), and may be separated by a comma, hyphen,
        /// or ampersand, with or without spaces (“2, 3”, “2-4”, “2 & 4”). For example, “2nd” tests “true” whereas “second”
        /// and “2nd edition” test “false”.
        /// </summary>
        [XmlAttribute("is-numeric")]
        public string? IsNumeric
        {
            get;
            set;
        }
        /// <summary>
        /// Tests whether the given date variables contain approximate dates.
        /// </summary>
        [XmlAttribute("is-uncertain-date")]
        public string? IsUncertainDate
        {
            get;
            set;
        }
        /// <summary>
        /// Tests whether the locator matches the given locator types (see Locators). Use “sub-verbo” to test for the “sub verbo” locator type.
        /// </summary>
        [XmlAttribute("locator")]
        public string? Locator
        {
            get;
            set;
        }
        /// <summary>
        /// Tests whether the cite position matches the given positions (terminology: citations consist of one or more cites to individual items). When called within the scope of cs:bibliography, position tests “false”.
        /// </summary>
        [XmlAttribute("position")]
        public string? Position
        {
            get;
            set;
        }
        /// <summary>
        /// Tests whether the item matches the given types.
        /// </summary>
        [XmlAttribute("type")]
        public string? Type
        {
            get;
            set;
        }
        /// <summary>
        /// Tests whether the default (long) forms of the given variables contain non-empty values.
        /// </summary>
        [XmlAttribute("variable")]
        public string? Variable
        {
            get;
            set;
        }

        /// <summary>
        /// Controls the testing logic.
        /// </summary>
        [XmlAttribute("match")]
        public Match Match
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'match' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool MatchSpecified
        {
            get;
            set;
        }

        public bool Matches(Citation citation)
        {
            List<bool> results = new List<bool>();

            var variableMatch = Matches(citation, Variable, (cit, split) => cit[split] != null);
            if (variableMatch != null)
                return variableMatch.Value;

            var numericMatch = Matches(citation, IsNumeric, (cit, split) => cit[split] is INumberVariable);
            if (numericMatch != null)
                return numericMatch.Value;

            var typeMatch = Matches(citation, Type, (cit, split) => cit["type"] is ITextVariable text && text.Value == split);
            if (typeMatch != null)
                return typeMatch.Value;

            var uncertainDateMatch = Matches(citation, IsUncertainDate, (cit, split) => cit[split] is IDateVariable date && date.IsApproximate);
            if (uncertainDateMatch != null)
                return uncertainDateMatch.Value;

            var locatorMatch = Matches(citation, Locator, (cit, split) => cit["locator"] is ITextVariable text && text.Value == split);
            if (locatorMatch != null)
                return locatorMatch.Value;

            var positionMatch = Matches(citation, Position, (cit, split) => throw new NotImplementedException());
            if (positionMatch != null)
                return positionMatch.Value;

            return false;
        }

        private bool? Matches(Citation citation, string? value, Func<Citation, string, bool> func)
        {
            if (value != null)
            {
                foreach (var split in value.ToLowerInvariant().Split(' '))
                {
                    var result = func(citation, split);

                    if (Match == Match.All)
                    {
                        if (!result)
                        {
                            return false;
                        }
                    }
                    else if (Match == Match.Any)
                    {
                        if (result)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (result)
                        {
                            return false;
                        }
                    }
                }
            }

            return null;
        }
    }
}
