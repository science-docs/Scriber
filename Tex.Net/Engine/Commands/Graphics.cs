﻿using System.IO;
using Tex.Net.Layout.Document;

namespace Tex.Net.Engine.Commands
{
    [Package]
    public static class Graphics
    {
        [Command("includegraphics")]
        public static ImageElement IncludeGraphics(CompilerState state, string imagePath, DocumentVariable? variables = null)
        {
            var bytes = File.ReadAllBytes(imagePath);
            var image = new ImageElement(bytes, imagePath);
            return image;
        }
    }
}
