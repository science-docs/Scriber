using System;
using System.Text.RegularExpressions;

namespace Scriber.Logging
{
    public static class SR
    {
        private static readonly Properties properties = new Properties();

        static SR()
        {
            var prefix = "Scriber.Resources.Messages.";
            var asm = typeof(SR).Assembly;
            var exceptions = asm.GetManifestResourceStream(prefix + "exceptions.properties");
            if (exceptions != null)
            {
                properties.Fill(exceptions);
            }
            var logging = asm.GetManifestResourceStream(prefix + "logging.properties");
            if (logging != null)
            {
                properties.Fill(logging);
            }
        }

        public static string Get(string name, params string[] args)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (properties.TryGetValue(name, out var property))
            {
                return string.Format(property, args);
            }
            throw new ArgumentException($"'{name}' is not a valid message entry", nameof(name));
        }

        public static bool Matches(string name, string message)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (properties.TryGetValue(name, out var property))
            {
                var regex = Regex.Escape(property);
                var last = string.Empty;
                int i = 0;
                while (last != regex)
                {
                    last = regex;
                    regex = regex.Replace("\\{" + i + "}", ".*");
                    i++;
                }
                return Regex.IsMatch(message, regex);
            }
            throw new ArgumentException($"'{name}' is not a valid message entry", nameof(name));
        }
    }
}
