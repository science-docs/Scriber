using ExCSS;
using Scriber.Engine;
using Scriber.Util;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Scriber.Layout.Styling
{
    public class StyleReader
    {
        private static readonly Context context = new Context();
        private static readonly StyleContainer[] defaultContainers;

        static StyleReader()
        {
            StyleKeys.Font.ToString();
            new ReflectionLoader().Discover(context, typeof(StyleReader).Assembly);
            defaultContainers = new StyleReader().Read(typeof(StyleReader).Assembly.GetManifestResourceStream("Scriber.Resources.Styles.Style.css")!);
        }

        public static StyleContainer[] GetDefaultStyles()
        {
            return defaultContainers;
        }

        public StyleContainer[] Read(Stream content, StyleOrigin origin = StyleOrigin.Author)
        {
            var parser = new StylesheetParser(true, true, true, true, true, false);
            var stylesheet = parser.Parse(content);
            var styles = new List<StyleContainer>(stylesheet.StyleRules.Count());

            foreach (var rule in stylesheet.StyleRules)
            {
                styles.Add(ToContainer(rule, origin));
            }

            return styles.ToArray();
        }

        public StyleContainer[] Read(string content, StyleOrigin origin = StyleOrigin.Author)
        {
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content), false);
            return Read(memoryStream, origin);
        }

        private StyleContainer ToContainer(IStyleRule rule, StyleOrigin origin)
        {
            var container = new StyleContainer(origin, rule.SelectorText);

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
    }
}
