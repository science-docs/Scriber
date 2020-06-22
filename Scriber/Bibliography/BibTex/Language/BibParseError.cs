using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scriber.Bibliography.BibTex.Language
{
    public class BibParseError
    {
        public int SourcePosition { get; }
        public int SourceLength { get; }

        public int LineCount { get; }
        public int ColCount { get; }

        public string MessageId { get; }
        public object[] MessageArgs { get; }

        public BibParseError(int pos, int len, int line, int col, string message, params object[] args)
        {
            SourcePosition = pos;
            SourceLength = len;
            LineCount = line;
            ColCount = col;
            MessageId = message;
            MessageArgs = args;
        }
    }
}
