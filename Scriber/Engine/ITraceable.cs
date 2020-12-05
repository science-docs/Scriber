using Scriber.Language.Syntax;
using System;

namespace Scriber.Engine
{
    public interface ITraceable<T> where T : SyntaxNode
    {
        T Origin { get; set; }
    }

    public abstract class Traceable : Traceable<SyntaxNode>
    {
        protected Traceable(SyntaxNode origin) : base(origin)
        {
        }
    }

    public abstract class Traceable<T> : ITraceable<T> where T: SyntaxNode
    {
        public T Origin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <exception cref="ArgumentNullException"/>
        protected Traceable(T origin)
        {
            Origin = origin ?? throw new ArgumentNullException(nameof(origin));
        }
    }
}
