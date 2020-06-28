using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scriber.Bibliography.Styling.Formatting
{
    public abstract class Run
    {
        protected Run(bool isEmpty)
        {
            // init
            IsEmpty = isEmpty;
        }

        public virtual bool IsEmpty
        {
            get;
            private set;
        }

        internal abstract IEnumerable<TextRun> GetTextRuns();

        public IEnumerable<TextRun> Flatten()
        {
            // get text runs
            var texts = GetTextRuns().ToArray();

            // group by formatting
            var index = 0;
            var text = new StringBuilder(4096);
            while (index < texts.Length)
            {
                // init
                var first = texts[index];
                text.Clear();

                // loop
                while (index < texts.Length && texts[index].IsFormatEqual(first))
                {
                    // append
                    text.Append(texts[index].Text);

                    // next
                    index++;
                }

                // done
                yield return new TextRun(text.ToString(), first.FontStyle, first.FontVariant, first.FontWeight, first.TextDecoration, first.VerticalAlign);
            }
        }

        public void ToPlainText(StringBuilder output)
        {
            foreach (var text in GetTextRuns())
            {
                output.Append(text.Text);
            }
        }
        public string ToPlainText()
        {
            // init
            var result = new StringBuilder(short.MaxValue);

            // render
            ToPlainText(result);

            // done
            return result.ToString();
        }

        public override string ToString()
        {
            return ToPlainText();
        }
    }
}