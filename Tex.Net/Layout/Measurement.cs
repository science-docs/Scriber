﻿using System;
using System.Collections.Generic;
using System.Text;
using Tex.Net.Layout.Document;

namespace Tex.Net.Layout
{
    public class Measurement
    {
        public Block Element { get; }
        public bool Flexible { get; set; }
        public double PagebreakPenalty { get; set; }
        public Size Size { get; }
        public Thickness Margin { get; }
        public int Index { get; internal set; } = -1;
        public Position Position { get; internal set; }

        public Measurement(Block element, Size size, Thickness margin)
        {
            Element = element;
            Size = size;
            Margin = margin;
        }
    }
}