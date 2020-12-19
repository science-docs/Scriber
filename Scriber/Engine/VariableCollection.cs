using System.Collections.Generic;

namespace Scriber.Engine
{
    public class VariableCollection
    {
        private readonly Dictionary<string, Variable> variables = new Dictionary<string, Variable>();

        public void Add(Variable variable)
        {
            variables[variable.Name] = variable;
        }

        public Variable? Get(string name)
        {
            if (variables.TryGetValue(name, out var variable))
            {
                return variable;
            }
            else
            {
                return null;
            }
        }
    }
}
