using Scriber.Language;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Engine
{
    public interface ITraceable
    {
        Element Origin { get; set; }
    }

    public abstract class Traceable : ITraceable
    {
        public Element Origin { get; set; }

        protected Traceable(Element origin)
        {
            Origin = origin ?? throw new ArgumentNullException(nameof(origin));
        }
    }
}
