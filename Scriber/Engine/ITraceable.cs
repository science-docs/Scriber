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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <exception cref="ArgumentNullException"/>
        protected Traceable(Element origin)
        {
            Origin = origin ?? throw new ArgumentNullException(nameof(origin));
        }
    }
}
