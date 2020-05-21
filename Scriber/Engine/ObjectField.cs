using Scriber.Language;

namespace Scriber.Engine
{
    public class ObjectField : Traceable
    {
        public string Key { get; }
        public object? Value { get; }

        public ObjectField(Element origin, string key, object? value) : base(origin)
        {
            Key = key;
            Value = value;
        }
    }
}
