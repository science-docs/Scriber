using System;
using System.Diagnostics.CodeAnalysis;

namespace Scriber.Util
{
    public class BitArray : IEquatable<BitArray>
    {
        public int Count { get; }

        private readonly byte[] bytes;

        public BitArray(int size)
        {
            Count = size;
            var byteCount = Count / 8;
            if (Count % 8 != 0)
            {
                byteCount++;
            }
            bytes = new byte[byteCount];
        }

        public void Set(int index, bool value)
        {
            var byteIndex = index / 8;
            var byteOffset = index % 8;
            var byteValue = value ? 1 : 0;
            var mask = byte.MaxValue ^ (1 << byteOffset);
            bytes[byteIndex] = (byte)(bytes[byteIndex] & mask | byteValue << byteOffset);
        }

        public bool Get(int index)
        {
            var byteIndex = index / 8;
            var byteOffset = index % 8;
            var byteValue = bytes[byteIndex] >> byteOffset & 1;
            return byteValue != 0;
        }

        public override bool Equals(object? obj)
        {
            if (obj is BitArray cacheStorage)
            {
                return Equals(cacheStorage);
            }
            return false;
        }

        public bool Equals([AllowNull] BitArray other)
        {
            if (other is null || bytes.Length != other.bytes.Length)
            {
                return false;
            }

            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] != other.bytes[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                int byteIndex = i % sizeof(int);
                hash ^= bytes[i] << (byteIndex * 8);
            }
            return hash;
        }

        public override string ToString()
        {
            var chars = new char[Count];
            for (int i = 0; i < Count; i++)
            {
                chars[i] = Get(i) ? '1' : '0';
            }
            return new string(chars);
        }
    }
}
