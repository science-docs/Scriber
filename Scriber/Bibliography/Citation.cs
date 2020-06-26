using System.Collections.Generic;

namespace Scriber.Bibliography
{
    public class Citation
    {
        public string Key { get; set; }

        private readonly Dictionary<string, IVariable> variables = new Dictionary<string, IVariable>();

        public IVariable? this[string name]
        {
            get => variables.TryGetValue(name.ToLowerInvariant(), out var value) ? value : null;
            set
            {
                if (value != null)
                {
                    variables[name.ToLowerInvariant()] = value;
                }
                else
                {
                    variables.Remove(name.ToLowerInvariant());
                }
            }
        }

        public Citation(string key)
        {
            Key = key;
        }

        public void CopyTo(IDictionary<string, IVariable> variables)
        {
            foreach (var variable in this.variables)
            {
                variables[variable.Key] = variable.Value;
            }
        }
    }
}
