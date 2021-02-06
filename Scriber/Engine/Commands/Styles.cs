using Scriber.Layout.Styling;
using Scriber.Util;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Styles
    {
        [Command("AddStyle")]
        public static void AddStyle(CompilerState state, string selector, DynamicDictionary style)
        {
            var styleSelector = StyleSelectorParser.Parse(selector);
            var container = new StyleContainer(styleSelector);

            foreach (var (name, value) in style.GetContents())
            {
                if (value == null)
                {
                    state.Context.Logger.Warning($"Style contains null value for property '{name}'.");
                    continue;
                }

                var propName = StringUtility.ToPascalCase(name);
                if (StyleKeys.TryGetByName(propName, out var styleKey) && state.Context.Converters.TryConvert(value, styleKey.Type, out var propertyValue))
                {
                    styleKey.Set(container, propertyValue);
                }
            }

            state.Document.Styles.Add(container);
        }

        [Command("IncludeCss")]
        public static void IncludeCss(CompilerState state, Argument<string> path)
        {
            var uri = state.Context.ResourceSet.RelativeUri(path.Value);
            var resource = state.Context.ResourceSet.Get(uri);
            var styleReader = new StyleReader();
            var styles = styleReader.Read(resource.GetContentAsString());

            foreach (var style in styles)
            {
                state.Document.Styles.Add(style);
            }
        }
    }
}
