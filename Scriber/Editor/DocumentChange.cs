﻿using System;

namespace Scriber.Editor
{
    public class DocumentChange
    {
        public int Start { get; set; }
        public int End { get; set; }
        public string Text { get; set; }

        public DocumentChange(string text)
        {
            Text = text;
        }

        public DocumentChange(string text, int start, int end)
        {
            Text = text;
            Start = start;
            End = end;
        }
    }
}
