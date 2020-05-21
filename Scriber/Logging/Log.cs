using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Logging
{
    public class Log
    {
        public LogLevel Level { get; set; }
        public string Message { get; set; }

        public Log(LogLevel level, string message)
        {
            Level = level;
            Message = message;
        }
    }
}
