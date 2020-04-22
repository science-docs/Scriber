using System.Collections;
using System.Collections.Generic;

namespace Tex.Net.Layout
{
    public class Measurements : IEnumerable<Measurement>
    {
        private readonly List<Measurement> measurements = new List<Measurement>();

        public int Count => measurements.Count;

        public bool IsReadOnly { get; private set; }

        public void Add(Measurement item)
        {
            if (!IsReadOnly)
            {
                if (item.Index < 0)
                {
                    item.Index = Count;
                }
                measurements.Add(item);
            }
        }

        public void Clear()
        {
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
    }
}
