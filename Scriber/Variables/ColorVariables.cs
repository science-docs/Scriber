using Scriber.Text;
using System.Collections.Generic;

namespace Scriber.Variables
{
    public static class ColorVariables
    {
        public static DocumentLocal<Dictionary<string, Color>> CustomColors { get; } = new DocumentLocal<Dictionary<string, Color>>();
    }
}
