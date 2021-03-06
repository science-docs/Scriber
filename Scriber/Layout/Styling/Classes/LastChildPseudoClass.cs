﻿using Scriber.Layout.Document;
using System.Linq;

namespace Scriber.Layout.Styling.Classes
{
    public class LastChildPseudoClass : PseudoClass
    {
        public override bool Matches(AbstractElement element)
        {
            var parent = element.Parent;
            if (parent is null)
                return false;

            var parentNodes = parent.ChildElements();
            return parentNodes.LastOrDefault() == element;
        }

        public override string ToString()
        {
            return "last-child";
        }
    }
}
