using System.Collections.Generic;

namespace Scriber.Bibliography
{
    public class Citation
    {
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

        public void CopyTo(IDictionary<string, IVariable> variables)
        {
            foreach (var variable in this.variables)
            {
                variables[variable.Key] = variable.Value;
            }
        }
    }
}
