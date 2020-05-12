using System.Collections;
using System.Collections.Generic;
using Tex.Net.Layout.Document;

namespace Tex.Net.Layout
{
    public class Measurements : IEnumerable<Measurement>
    {
        private readonly List<Measurement> measurements = new List<Measurement>();

        public int Count => measurements.Count;

        public bool IsReadOnly { get; private set; }

        public Measurement this[int index] => measurements[index];

        private int count = 0;

        public void Add(Measurement item)
        {
            if (!IsReadOnly)
            {
                if (item.Index < 0)
                {
                    item.Index = count++;
                }
                measurements.Add(item);
            }
        }

        public void Clear()
        {
            count = 0;
            measurements.Clear();
        }

        internal void AddInternal(Measurements measurements)
        {
            this.measurements.AddRange(measurements);
        }

        public bool Contains(Measurement item)
        {
            return measurements.Contains(item);
        }

        public void Finish()
        {
            IsReadOnly = true;
        }

        public int IndexOf(Measurement item)
        {
            return measurements.IndexOf(item);
        }

        public IEnumerator<Measurement> GetEnumerator()
        {
            return measurements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return measurements.GetEnumerator();
        }

        public static Measurements Singleton(Measurement measurement)
        {
            var measurements = new Measurements
            {
                measurement
            };
            return measurements;
        }

        public static Measurements EmptySingleton(DocumentElement documentElement)
        {
            return Singleton(new Measurement(documentElement, Size.Zero, Thickness.Zero));
        }
    }
}
