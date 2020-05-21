using System.Diagnostics;
using Scriber.Engine;

namespace Scriber.CLI
{
    [Package("test")]
    public static class TestPackage
    {
        [Command("test")]
        public static void Test([Argument] TestObject obj)
        {
            if (obj != null)
            {
                //Debug.WriteLine("A: " + obj.A + " B: " + obj.B);
            }
            else
            {
                Debug.WriteLine("Obj is null");
            }
            //Debug.WriteLine(string.Join(", ", array) + ", " + value);
        }

        public class TestObject
        {
            [ObjectField("a")]
            public string A { get; set; }
            [ObjectField("key")]
            public string B { get; set; }
            [ObjectField("next")]
            public TestObject Next { get; set; }
        }
    }
}
