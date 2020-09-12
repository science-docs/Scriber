using Scriber.Engine;
using System;

namespace Scriber.Tests.Fixture
{
    public class SimpleObject
    {
        public int A { get; set; }
        [ObjectField("b")]
        public string? B { get; set; }
        public SimpleObject? Other { get; set; }

        public static void SimpleMethod(SimpleObject simple)
        {
            simple.ToString();
        }

        public static void OverrideMethod([Argument(Overrides = new Type[] { typeof(OverrideObject) })] SimpleObject overrode)
        {
            overrode.ToString();
        }
    }

    public class OverrideObject : SimpleObject
    {

    }
}
