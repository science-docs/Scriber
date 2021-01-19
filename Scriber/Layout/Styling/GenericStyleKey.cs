namespace Scriber.Layout.Styling
{
    public class StyleKey<T> : StyleKey
    {
        public StyleKey(string name, T defaultValue) : base(name, typeof(T), defaultValue)
        {
        }

        public StyleKey(string name, bool inherited, T defaultValue) : base(name, typeof(T), inherited, defaultValue)
        {
        }
    }
}
