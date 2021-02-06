using System;
using System.Runtime.InteropServices;

namespace Scriber.Layout.Styling
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, CharSet = CharSet.Unicode)]
    public struct Priority : IEquatable<Priority>, IComparable<Priority>
    {
        [FieldOffset(0)] private readonly byte tags;
        [FieldOffset(1)] private readonly byte classes;
        [FieldOffset(0)] private readonly ushort priority;

        public static readonly Priority Zero = new Priority(0);
        public static readonly Priority OneTag = new Priority(0, 1);
        public static readonly Priority OneClass = new Priority(1, 0);

        public Priority(ushort priority)
        {
            classes = tags = 0;
            this.priority = priority;
        }

        public Priority(byte classes, byte tags)
        {
            priority = 0;
            this.classes = classes;
            this.tags = tags;
        }

        public byte Tags => tags;
        public byte Classes => classes;

        public static Priority operator +(Priority a, Priority b)
        {
            return new Priority((ushort)(a.priority + b.priority));
        }

        public static bool operator ==(Priority a, Priority b)
        {
            return a.priority == b.priority;
        }
        public static bool operator >(Priority a, Priority b)
        {
            return a.priority > b.priority;
        }

        public static bool operator >=(Priority a, Priority b)
        {
            return a.priority >= b.priority;
        }

        public static bool operator <(Priority a, Priority b)
        {
            return a.priority < b.priority;
        }

        public static bool operator <=(Priority a, Priority b)
        {
            return a.priority <= b.priority;
        }

        public static bool operator !=(Priority a, Priority b)
        {
            return a.priority != b.priority;
        }

        public bool Equals(Priority other)
        {
            return priority == other.priority;
        }

        public override bool Equals(object? obj)
        {
            return obj is Priority prio && Equals(prio);
        }

        public override int GetHashCode()
        {
            return priority;
        }

        public int CompareTo(Priority other)
        {
            return priority.CompareTo(other.priority);
        }

        public override string ToString()
        {
            return $"({classes}, {tags})";
        }
    }
}
