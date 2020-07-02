using System.Collections.Generic;
using System.IO;

namespace Scriber.Logging
{
    public class Properties : Dictionary<string, string>
    {
        public Properties() : base()
        {
        }

        public void Fill(Stream stream)
        {
            using var sr = new StreamReader(stream);
            Fill(sr);
        }

        public void Fill(StreamReader reader)
        {
            Fill(reader.ReadToEnd());
        }

        public void Fill(string content)
        {
            var lines = content.Split('\n');
            foreach (var line in lines)
            {
                int index = line.IndexOf('=');
                if (index >= 0)
                {
                    var key = line.Substring(0, index).Trim();
                    var value = line.Substring(index + 1).Trim();
                    this[key] = value;
                }
            }
        }
    }
}
