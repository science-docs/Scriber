using Scriber.Drawing;
using Scriber.Layout.Styling;
using Img = SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>;

namespace Scriber.Layout.Document
{
    public class ImageElement : DocumentElement
    {
        private readonly byte[] imageData;
        public Img Image { get; }

        public Unit Height
        {
            get => height ?? OriginalHeight;
            set => height = value;
        }
        public Unit Width
        {
            get => width ?? OriginalWidth;
            set => width = value;
        }
        public Unit OriginalHeight { get; set; }
        public Unit OriginalWidth { get; set; }
        public double Scale { get; set; } = 1.0;
        public double Angle { get; set; } = 0.0;
        public Position Origin { get; set; }

        public string FileName { get; }

        public bool KeepAspectRatio { get; set; } = true;
        public bool Draft { get; set; } = false;

        private Unit? height;
        private Unit? width;

        public ImageElement(byte[] imageData, string fileName) : this(null, imageData, fileName)
        {
            
        }

        private ImageElement(Img? image, byte[] imageData, string fileName)
        {
            Tag = "img";
            FileName = fileName;
            this.imageData = imageData;
            Image = image ?? SixLabors.ImageSharp.Image.Load(imageData);
            OriginalHeight = new Unit(Image.Height, UnitType.Presentation);
            OriginalWidth = new Unit(Image.Width, UnitType.Presentation);
            Origin = new Position(OriginalHeight.Presentation / 2, OriginalWidth.Presentation / 2);
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
                height = height,
                width = width,
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
            if (KeepAspectRatio)
            {
                if (width != null)
                {
                    var aspectRatio = OriginalWidth.Presentation / OriginalHeight.Presentation;
                    height = new Unit(width.Value.Presentation / aspectRatio, UnitType.Presentation);
                }
                else if (height != null)
                {
                    var aspectRatio = OriginalWidth.Presentation / OriginalHeight.Presentation;
                    width = new Unit(height.Value.Presentation * aspectRatio, UnitType.Presentation);
                }
            }

            var size = new Size(Width.Point * Scale, Height.Point * Scale);
            var margin = Style.Get(StyleKeys.Margin);
            if (size.Width > availableSize.Width)
            {
                var ratio = size.Width / (availableSize.Width - margin.Width - 2);
                size.Width /= ratio;
                size.Height /= ratio;
            }
            return new Measurement(this, size, margin);
        }

        public override SplitResult Split(Measurement source, double height)
        {
            return new SplitResult(source, source, null);
        }
    }
}
