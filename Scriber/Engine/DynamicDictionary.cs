using System.Collections.Generic;
using System.Dynamic;

namespace Scriber.Engine
{
    public class DynamicDictionary : DynamicObject
    {
        private readonly Dictionary<string, object?> dictionary = new Dictionary<string, object?>();

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
            dictionary[name.ToLowerInvariant()] = value;
        }
    }
}
