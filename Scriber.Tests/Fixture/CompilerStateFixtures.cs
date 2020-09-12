using Scriber.Engine;

namespace Scriber.Tests.Fixture
{
    public static class CompilerStateFixtures
    {
        public static CompilerState ReflectionLoaded()
        {
            var loader = new ReflectionLoader();
            var context = new Context();
            loader.Discover(context, typeof(CompilerStateFixtures).Assembly);
            return new CompilerState(context);
        }
    }
}
