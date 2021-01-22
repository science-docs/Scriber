using System;

namespace Scriber.Layout.Styling
{
    public interface IStyleKey
    {
        string Name { get; }
        bool Inherited { get; }
        Type Type { get; }

        object Get(Style style);
        void Set(StyleContainer styleContainer, object value);
    }
}
