using Scriber.Engine;
using System.Threading;

namespace Scriber.Tests.Fixture
{
    public static class CompilerStateFixtures
    {
        public static CompilerState Empty()
        {
            return new CompilerState(new Context(), CancellationToken.None);
        }

        public static CompilerState ReflectionLoaded()
        {
            var loader = new ReflectionLoader();
            var context = new Context();
            loader.Discover(context, typeof(CompilerStateFixtures).Assembly);
            return new CompilerState(context, CancellationToken.None);
        }
    }
}
