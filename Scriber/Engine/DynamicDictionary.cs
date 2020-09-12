using System.Collections.Generic;
using System.Dynamic;

namespace Scriber.Engine
{
    public class DynamicDictionary : DynamicObject
    {
        private readonly Dictionary<string, object?> dictionary = new Dictionary<string, object?>();
        private readonly Dictionary<string, string> names = new Dictionary<string, string>();

        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            string name = binder.Name.ToLowerInvariant();
            return dictionary.TryGetValue(name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object? value)
        {
            SetMember(binder.Name, value);
            return true;
        }

        public void SetMember(string name, object? value)
        {
            names[name.ToLowerInvariant()] = name;
            dictionary[name.ToLowerInvariant()] = value;
        }

        public IEnumerable<KeyValuePair<string, object?>> GetContents()
        {
            foreach (var entry in dictionary)
            {
                yield return new KeyValuePair<string, object?>(names[entry.Key], entry.Value);
            }
        }
    }
}
