using System.Collections.Generic;
using System.Linq;

namespace Scriber.Bibliography.Styling.Formatting
{
    public sealed class ComposedRun : Run
    {
        internal ComposedRun(string tag, IEnumerable<Run> children, bool byVariable)
            : base(children.All(x => x.IsEmpty))
        {
            // init
            Tag = tag;
            Children = new List<Run>(children);
            ByVariable = byVariable;
        }

        public override bool IsEmpty => Children.All(x => x.IsEmpty);

        public string Tag
        {
            get;
            private set;
        }

        public List<Run> Children { get; }
        internal bool ByVariable
        {
            get;
            private set;
        }

        internal override IEnumerable<TextRun> GetTextRuns()
        {
            return Children
                .SelectMany(x => x.GetTextRuns());
        }
    }
}
