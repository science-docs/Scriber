using System.Diagnostics;
using Scriber.Engine;

namespace Scriber.CLI
{
    [Package("test")]
    public static class TestPackage
    {
        [Command("test")]
        public static void Test(string[] array, string value, TestObject obj)
        {
            Debug.WriteLine("A: " + obj.A + " B: " + obj.B);
            Debug.WriteLine(string.Join(", ", array) + ", " + value);
        }

        public class TestObject
        {
            [ObjectField("a")]
            public string A { get; set; }
            [ObjectField("key")]
            public string B { get; set; }
        }
    }
}
