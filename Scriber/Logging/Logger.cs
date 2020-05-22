namespace Scriber.Logging
{
    public class Logger
    {
        public LogLevel Level { get; set; } = LogLevel.Info;

        public event LogEvent? Logged;

        public void Log(LogLevel level, string message)
        {
            Log log = new Log(level, message);
            if ((int)level >= (int)Level)
            {
                Logged?.Invoke(log);
            }
        }

        public void Debug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        public void Info(string message)
        {
            Log(LogLevel.Info, message);
        }

        public void Warning(string message)
        {
            Log(LogLevel.Warning, message);
        }

        public void Error(string message)
        {
            Log(LogLevel.Error, message);
        }
    }
}
