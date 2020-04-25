using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;

namespace Tex.Net.Drawing
{
    public class Image
    {
        public double Height { get; set; }
        public double Width { get; set; }

        private readonly byte[] data;
        private readonly SixLabors.ImageSharp.Image<Rgba32> image;

        public Image(byte[] data)
        {
            this.data = data;
            image = SixLabors.ImageSharp.Image.Load(data);
        }

        public Image(byte[] data, SixLabors.ImageSharp.Image<Rgba32> image)
        {
            this.data = data;
            this.image = image;
        }

        public byte[] GetData()
        {
            return data;
        }

        public Stream GetStream()
        {
            // currently, a workaround is needed for jpg images.
            // so all images are saved as png.
            var ms = new MemoryStream();
            image.Save(ms, new PngEncoder());
            ms.Position = 0;
            return ms;

            //if (data == null)
            //{
            //    var ms = new MemoryStream();
            //    image.Save(ms, new PngEncoder());
            //    ms.Position = 0;
            //    return ms;
            //}
            //else
            //{
            //    return new MemoryStream(data);
            //}
        }

        public static Image FromStream(Stream stream)
        {
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return new Image(ms.ToArray());
        }
    }
}
