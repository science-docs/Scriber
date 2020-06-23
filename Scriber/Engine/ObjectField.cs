using Scriber.Language;

namespace Scriber.Engine
{
    public class ObjectField : Traceable
    {
        public string Key { get; }
        public Argument Argument { get; }

        public ObjectField(Element origin, string key, Argument value) : base(origin)
        {
            Key = key;
            Argument = value;
        }
    }
}
