using Scriber.Autocomplete;
using Scriber.Layout;
using Scriber.Layout.Document;
using System.IO;
using System.Net.Http;

namespace Scriber.Engine.Commands
{
    [Package]
    public static class Graphics
    {
        [Command("Caption")]
        public static Paragraph Caption(Paragraph paragraph)
        {
            paragraph.Tag = "caption";
            paragraph.Margin = new Thickness(16, 0, 0, 0);
            return paragraph;
        }

        [Command("IncludeGraphics")]
        public static AbstractElement IncludeGraphics(CompilerState state, [Argument(Name = "image", ProposalProvider = typeof(ImageFileProposalProvider))] Argument<string> imagePath, IncludeImageOptions? options = null)
        {
            if (options == null || !options.Draft)
            {
                try
                {
                    var bytes = state.Context.ResourceSet.RelativeResource(imagePath.Value).GetContent();
                    var image = new ImageElement(bytes, imagePath.Value);

                    if (options != null)
                    {
                        image.Angle = options.Angle;
                        image.Scale = options.Scale;
                        image.Height = options.Height;
                        image.Width = options.Width;
                    }

                    return image;
                }
                catch (IOException io)
                {
                    state.Issues.Add(imagePath.Source, CompilerIssueType.Warning, $"Could not read file {imagePath.Value}. Make sure it does exist and is accessible.", io);
                    return Paragraph.FromText(imagePath.Value);
                }
                catch (HttpRequestException http)
                {
                    state.Issues.Add(imagePath.Source, CompilerIssueType.Warning, $"Could not download resource from {imagePath.Value}.", http);
                    return Paragraph.FromText(imagePath.Value);
                }
            }
            else
            {
                return Paragraph.FromText(imagePath.Value);
            }
        }

        public class IncludeImageOptions
        {
            public double Angle { get; set; }
            public bool Draft { get; set; }
            public double Scale { get; set; }
            public Unit Height { get; set; }
            public Unit Width { get; set; }
        }
    }
}
