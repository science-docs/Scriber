using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Maths
{
    public class MathResult
    {
        public string? Expression { get; set; }
        public double Value { get; set; }

        public MathResult(double value, string? expression)
        {
            Expression = expression;
            Value = value;
        }

        public double AsDouble()
        {
            return Value;
        }

        public byte AsByte()
        {
            if (Value < byte.MinValue || Value > byte.MaxValue)
            {
                throw new OverflowException();
            }
            return (byte)Value;
        }

        public sbyte AsSByte()
        {
            if (Value < sbyte.MinValue || Value > sbyte.MaxValue)
            {
                throw new OverflowException();
            }
            return (sbyte)Value;
        }

        public short AsShort()
        {
            if (Value < short.MinValue || Value > short.MaxValue)
            {
                throw new OverflowException();
            }
            return (short)Value;
        }

        public ushort AsUShort()
        {
            if (Value < ushort.MinValue || Value > ushort.MaxValue)
            {
                throw new OverflowException();
            }
            return (ushort)Value;
        }

        public int AsInt()
        {
            if (Value < int.MinValue || Value > int.MaxValue)
            {
                throw new OverflowException();
            }
            return (int)Value;
        }

        public uint AsUInt()
        {
            if (Value < uint.MinValue || Value > uint.MaxValue)
            {
                throw new OverflowException();
            }
            return (uint)Value;
        }

        public long AsLong()
        {
            if (Value < long.MinValue || Value > long.MaxValue)
            {
                throw new OverflowException();
            }
            return (long)Value;
        }

        public ulong AsULong()
        {
            if (Value < ulong.MinValue || Value > ulong.MaxValue)
            {
                throw new OverflowException();
            }
            return (ulong)Value;
        }

        public float AsSingle()
        {
            if (Value < float.MinValue || Value > float.MaxValue)
            {
                throw new OverflowException();
            }
            return (float)Value;
        }

        public decimal AsDecimal()
        {
            return (decimal)Value;
        }
    }
}
