using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tex.Net.Drawing
{
    public class Image
    {
        public double Height { get; private set; }
        public double Width { get; private set; }

        private readonly byte[] data;

        public Image(byte[] data)
        {
            this.data = data;
            //var img = new System.Drawing.Image()
        }

        public byte[] GetData()
        {
            return data;
        }

        public Stream GetStream()
        {
            return new MemoryStream(data);
        }

        public static Image FromStream(Stream stream)
        {
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return new Image(ms.ToArray());
        }
    }
}
