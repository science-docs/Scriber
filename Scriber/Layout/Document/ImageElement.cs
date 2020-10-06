﻿using Scriber.Drawing;

using Img = SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>;

namespace Scriber.Layout.Document
{
    public class ImageElement : DocumentElement
    {
        private readonly byte[] imageData;
        public Img Image { get; }

        public Unit Height { get; set; }
        public Unit Width { get; set; }
        public double Scale { get; set; } = 1.0;
        public double Angle { get; set; } = 0.0;
        public Position Origin { get; set; }

        public string FileName { get; }

        public bool KeepAspectRatio { get; set; } = true;
        public bool Draft { get; set; } = false;

        public ImageElement(byte[] imageData, string fileName) : this(null, imageData, fileName)
        {
            
        }

        private ImageElement(Img? image, byte[] imageData, string fileName)
        {
            FileName = fileName;
            this.imageData = imageData;
            Image = image ?? SixLabors.ImageSharp.Image.Load(imageData);
            Height = new Unit(Image.Height, UnitType.Presentation);
            Width = new Unit(Image.Width, UnitType.Presentation);
            Origin = new Position(Width.Presentation / 2, Height.Presentation / 2);
        }

        protected override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            var image = new Image(imageData, Image);
            drawingContext.DrawImage(image, new Rectangle(Position.Zero, measurement.Size));
        }

        protected override AbstractElement CloneInternal()
        {
            var image = new ImageElement(Image, imageData, FileName)
            {
                Height = Height,
                Width = Width,
                Scale = Scale,
                Angle = Angle,
                Origin = Origin,
                KeepAspectRatio = KeepAspectRatio,
                Draft = Draft
            };
            return image;
        }

        protected override Measurement MeasureOverride(Size availableSize)
        {
            var size = new Size(Width.Point * Scale, Height.Point * Scale);
            if (size.Width > availableSize.Width)
            {
                var ratio = size.Width / (availableSize.Width - Margin.Width - 2);
                size.Width /= ratio;
                size.Height /= ratio;
            }
            return new Measurement(this, size, Margin);
        }

        public override SplitResult Split(Measurement source, double height)
        {
            return new SplitResult(source, source, null);
        }
    }
}
