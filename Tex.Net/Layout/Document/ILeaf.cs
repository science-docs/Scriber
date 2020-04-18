using System;
using System.Collections.Generic;
using System.Text;

namespace Tex.Net.Layout.Document
{
    public interface ILeaf
    {
        Size DesiredSize { get; }
        Size Measure(Size availableSize);
        LineNode[] GetNodes();
    }
}
