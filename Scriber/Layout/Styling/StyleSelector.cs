using Scriber.Layout.Document;
using System.Collections.Generic;

namespace Scriber.Layout.Styling
{
    public abstract class StyleSelector
    {
        public abstract bool Matches(AbstractElement element);

        public static IEnumerable<StyleSelector> FromString(string path)
        {
            var splits = path.Split(',');

            for (int i = 0; i < splits.Length; i++)
            {
                var split = splits[i].Trim();
                if (split == "*")
                {
                    yield return new AllStyleSelector();
                }
                else if (split.StartsWith("."))
                {
                    yield return new ClassStyleSelector(split.Substring(1));
                }
                else
                {
                    yield return new TagStyleSelector(split);
                }
            }
        }
    }
}
