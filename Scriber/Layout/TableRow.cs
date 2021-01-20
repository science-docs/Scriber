using Scriber.Drawing;
using Scriber.Layout.Document;
using Scriber.Layout.Styling;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Scriber.Layout
{
    public class TableRow : DocumentElement, IList<DocumentElement>
    {
        private List<DocumentElement> data { get; } = new List<DocumentElement>();

        public int Width => data.Count;

        public int Count => data.Count;

        public bool IsReadOnly => false;

        public DocumentElement this[int index] 
        { 
            get => data[index]; 
            set {
                value.Parent = this;
                value.Tag = "td";
                data[index] = value;
            }
        }

        public TableRow()
        {
            Tag = "tr";
        }

        protected override Measurement MeasureOverride(Size availableSize)
        {
            var margin = Style.Get(StyleKeys.Margin);
            var measurement = new Measurement(this, null, margin);

            foreach (var td in data)
            {
                measurement.Subs.Add(td.Measure(availableSize));
            }

            return measurement;
        }

        protected override void OnRender(IDrawingContext drawingContext, Measurement measurement)
        {
            foreach (var sub in measurement.Subs)
            {
                sub.Element.Render(drawingContext, sub);
            }
        }

        public new TableRow Clone()
        {
            if (!(base.Clone() is TableRow row))
            {
                throw new InvalidCastException();
            }

            return row;
        }

        protected override AbstractElement CloneInternal()
        {
            var row = new TableRow();
            
            foreach (var data in data)
            {
                row.data.Add(data.Clone());
            }

            return row;
        }

        public int IndexOf(DocumentElement item)
        {
            return data.IndexOf(item);
        }

        public void Insert(int index, DocumentElement item)
        {
            item.Tag = "td";
            data.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            data.RemoveAt(index);
        }

        public void Add(DocumentElement item)
        {
            item.Tag = "td";
            data.Add(item);
        }

        public void AddRange(IEnumerable<DocumentElement> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void Clear()
        {
            data.Clear();
        }

        public bool Contains(DocumentElement item)
        {
            return data.Contains(item);
        }

        public void CopyTo(DocumentElement[] array, int arrayIndex)
        {
            data.CopyTo(array, arrayIndex);
        }

        public bool Remove(DocumentElement item)
        {
            return data.Remove(item);
        }

        public IEnumerator<DocumentElement> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
