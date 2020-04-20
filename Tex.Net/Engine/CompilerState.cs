using System.Collections.Generic;
using Tex.Net.Language;
using Tex.Net.Util;

namespace Tex.Net.Engine
{
    public class CompilerState
    {
        public Document Document { get; }
        public EnvironmentTree Environments { get; }
        public List<CompilerIssue> Issues { get; } = new List<CompilerIssue>();

        public CompilerState()
        {
            Document = new Document();
            Environments = new EnvironmentTree();
        }

        public object Execute(Element element, object[] arguments)
        {
            var instruction = EngineInstruction.Create(element);

            var result = instruction?.Execute(this, arguments);

            //switch (result)
            //{
            //    case Document doc:
            //        Document = doc;
            //        break;
            //}

            if (result != null)
            {
                var flattened = result.ConvertToFlatArray();
                Environments.Current.Objects.AddRange(flattened);
            }

            return result;
        }

        
    }
}
