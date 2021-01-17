using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Scriber.Bibliography
{
    public static class CitationJsonParser
    {
        public static Citation Parse(JsonElement json)
        {
            var properties = json.EnumerateObject();
            var id = string.Empty;
            if (json.TryGetProperty("id", out var idElement))
            {
                id = idElement.ToString();
            }
            var type = string.Empty;
            if (json.TryGetProperty("type", out var typeElement))
            {
                type = typeElement.ToString();
            }
            var cb = new CitationBuilder(id, type);

            foreach (var property in properties)
            {
                IVariable variable;
                switch (property.Name)
                {
                    case "chapter-number":
                    case "collection-number":
                    case "edition":
                    case "issue":
                    case "number":
                    case "number-of-pages":
                    case "number-of-volumes":
                    case "volume":
                        variable = ParseNumber(property.Value);
                        break;
                    case "accessed":
                    case "container":
                    case "event-date":
                    case "issued":
                    case "original-date":
                    case "submitted":
                        variable = ParseDate(property.Value);
                        break;
                    case "author":
                    case "collection-author":
                    case "composer":
                    case "container-author":
                    case "director":
                    case "editor":
                    case "editorial-director":
                    case "illustrator":
                    case "interviewer":
                    case "original-author":
                    case "recipient":
                    case "reviewed-author":
                    case "translator":
                        variable = ParseName(property.Value);
                        break;
                    default:
                        variable = new TextVariable(property.Value.ToString());
                        break;
                }
                cb.Set(variable, property.Name);
            }

            return cb.Build();
        }

        private static IDateVariable ParseDate(JsonElement json)
        {
            int fromYear = 0;
            int? fromMonth = null;
            int? fromDay = null;

            int? toYear = null;
            int? toMonth = null;
            int? toDay = null;

            if (json.TryGetProperty("date-parts", out var dateParts))
            {
                var fromToParts = dateParts.EnumerateArray();
                if (fromToParts.MoveNext())
                {
                    var from = fromToParts.Current.EnumerateArray();
                    from.MoveNext();
                    fromYear = GetInt32(from.Current) ?? 0;
                    if (from.MoveNext())
                    {
                        fromMonth = GetInt32(from.Current);

                        if (from.MoveNext())
                        {
                            fromDay = GetInt32(from.Current);
                        }
                    }

                    if (fromToParts.MoveNext())
                    {
                        var to = fromToParts.Current.EnumerateArray();
                        to.MoveNext();
                        toYear = GetInt32(to.Current);
                        if (to.MoveNext())
                        {
                            toMonth = GetInt32(to.Current);

                            if (to.MoveNext())
                            {
                                toDay = GetInt32(to.Current);
                            }
                        }
                    }
                }
            }
            
            //if (json.TryGetProperty("season", out var seasonElement))
            //{

            //}

            return new DateVariable(fromYear, null, fromMonth, fromDay, toYear ?? fromYear, null, toMonth, toDay, false);
        }

        private static INamesVariable ParseName(JsonElement json)
        {
            var names = new List<IName>();
            foreach (var nameElement in json.EnumerateArray())
            {
                if (nameElement.TryGetProperty("given", out var givenName))
                {
                    var familyName = string.Empty;
                    if (nameElement.TryGetProperty("family", out var familyNameElement))
                    {
                        familyName = familyNameElement.GetString();
                    }
                    names.Add(new PersonalName(familyName, givenName.GetString()));
                }
                else if (nameElement.TryGetProperty("family", out var familyNameElement))
                {
                    var familyName = familyNameElement.GetString();
                    names.Add(new PersonalName(familyName, string.Empty));
                }
                else if (nameElement.TryGetProperty("literal", out var literalName))
                {
                    names.Add(new InstitutionalName(literalName.GetString()));
                }
                else
                {
                    throw new Exception();
                }
            }

            return new NamesVariable(names);
        }

        private static INumberVariable ParseNumber(JsonElement json)
        {
            NumberVariable.TryParse(json.ToString(), out var number);
            return number;
        }

        private static int? GetInt32(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Number)
            {
                return element.GetInt32();
            }
            else if (element.ValueKind == JsonValueKind.String && int.TryParse(element.GetString(), out var intValue))
            {
                return intValue;
            }
            else
            {
                return null;
            }
        }
    }
}
