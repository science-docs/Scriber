using Scriber.Layout.Styling;
using Scriber.Util;
using System;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Styles
    {
        [Command("AddStyle")]
        public static void AddStyle(CompilerState state, Argument<string> selector, DynamicDictionary style)
        {
            IStyleSelector styleSelector;
            try
            {
                styleSelector = StyleSelectorParser.Parse(selector.Value);
            }
            catch (Exception e)
            {
                state.Issues.Add(selector.Source, CompilerIssueType.Warning, e.Message);
                return;
            }
            
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
            var uri = state.Context.ResourceManager.RelativeUri(path.Value);
            var resource = state.Context.ResourceManager.Get(uri);
            var styleReader = new StyleReader();
            var styles = styleReader.Read(resource.GetContentAsString());

            foreach (var style in styles)
            {
                state.Document.Styles.Add(style);
            }
        }
    }
}
