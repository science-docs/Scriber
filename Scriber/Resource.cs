using System;
using System.Text;

namespace Scriber
{
    public class Resource
    {
        public Uri Uri { get; }

        private byte[] content;

        public Resource(Uri uri, byte[] content)
        {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            this.content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public Resource(Uri uri, string content)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            this.content = Encoding.UTF8.GetBytes(content);
        }

        public byte[] GetContent()
        {
            return content;
        }

        public string GetContentAsString()
        {
            if (content.Length > 3 && content[0] == 0xEF && content[1] == 0xBB && content[2] == 0xBF)
            {
                return Encoding.UTF8.GetString(content[3..]);
            }
            return Encoding.UTF8.GetString(content);
        }

        public void SetContent(byte[] content)
        {
            this.content = content;
        }

        public void SetContent(string content)
        {
            this.content = Encoding.UTF8.GetBytes(content);
        }
    }
}
