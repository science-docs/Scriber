using System;

namespace Scriber.Layout.Styling
{
    public class ComputedStyleKey<T>
    {
        public string Name { get; set; }
        public bool Inherited { get; set; }
        public Func<Style, T> Compute { get; set; }

        public ComputedStyleKey(string name, Func<Style, T> compute)
        {
            Name = name;
            Inherited = false;
            Compute = compute;
        }

        public ComputedStyleKey(string name, bool inherited, Func<Style, T> compute)
        {
            Name = name;
            Inherited = inherited;
            Compute = compute;
        }
    }
}
