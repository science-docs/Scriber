using System;
using System.Globalization;
using System.Linq;

namespace Scriber.Logging
{
    public class Log
    {
        public LogLevel Level { get; }
        public string Message { get; }
        public DateTime Timestamp { get; }

        private static readonly int LongestLevelName = Enum.GetNames(typeof(LogLevel)).Max(e => e.Length);

        public Log(LogLevel level, string message)
        {
            Level = level;
            Message = message;
            Timestamp = DateTime.Now;
        }

        public string[] GetFullMessageParts()
        {
            string levelString = Level.ToString().ToUpper(CultureInfo.InvariantCulture);
            var diff = LongestLevelName - levelString.Length;

            var pad = new string(' ', diff);

            return new string[]
            {
                "[",
                levelString,
                $"]{pad} {Message}"
            };
        }
    }
}
