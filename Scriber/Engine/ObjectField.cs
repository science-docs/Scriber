using Scriber.Language;
using Scriber.Language.Syntax;

namespace Scriber.Engine
{
    public class ObjectField : Traceable<FieldSyntax>
    {
        public string Key { get; }
        public Argument Argument { get; }

        public ObjectField(FieldSyntax origin, string key, Argument value) : base(origin)
        {
            Key = key;
            Argument = value;
        }
    }
}
