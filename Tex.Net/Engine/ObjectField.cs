namespace Tex.Net.Engine
{
    public class ObjectField
    {
        public string Key { get; }
        public object Value { get; }

        public ObjectField(string key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}
