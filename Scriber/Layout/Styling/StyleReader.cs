using ExCSS;
using Scriber.Engine;
using Scriber.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Scriber.Layout.Styling
{
    public class StyleReader
    {
        private static readonly Context context = new();
        private static readonly Lazy<StyleContainer[]> defaultContainers = new(LoadDefaultStyles);

        public static StyleContainer[] GetDefaultStyles()
        {
            return defaultContainers.Value;
        }

        public StyleContainer[] Read(Stream content)
        {
            var parser = new StylesheetParser(true, true, true, true, true, false);
            var stylesheet = parser.Parse(content);
            var styles = new List<StyleContainer>(stylesheet.StyleRules.Count());

            foreach (var rule in stylesheet.StyleRules)
            {
                styles.Add(ToContainer(rule));
            }

            return styles.ToArray();
        }

        public StyleContainer[] Read(string content)
        {
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content), false);
            return Read(memoryStream);
        }

        private StyleContainer ToContainer(IStyleRule rule)
        {
            var container = new StyleContainer(rule.SelectorText);

            foreach (var property in rule.Style)
            {
                var propName = StringUtility.ToPascalCase(property.Name);
                if (StyleKeys.TryGetByName(propName, out var styleKey) && context.Converters.TryConvert(property.Value, styleKey.Type, out var propertyValue))
                {
                    styleKey.Set(container, propertyValue);
                }
            }

            return container;
        }

        private static StyleContainer[] LoadDefaultStyles()
        {
            // Force creating all style keys
            StyleKeys.Font.ToString();
            new ReflectionLoader().Discover(context, typeof(StyleReader).Assembly);
            var defaultContainers = new StyleReader().Read(typeof(StyleReader).Assembly.GetManifestResourceStream("Scriber.Resources.Styles.Style.css")!);
            return defaultContainers;
        }
    }
}
