using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Scriber.Util.Tests
{
    public class BitArrayTests
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(4, 0, 2, 3)]
        [InlineData(8, 0, 7)]
        [InlineData(10, 1, 7, 8, 9)]
        public void TestSetterAndGetter(int length, params int[] trueIndices)
        {
            var bitArray = new BitArray(length);
            Assert.Equal(length, bitArray.Count);
            for (int i = 0; i < length; i++)
            {
                if (trueIndices.Contains(i))
                {
                    bitArray.Set(i, true);
                }
            }

            for (int i = 0; i < length; i++)
            {
                Assert.Equal(trueIndices.Contains(i), bitArray.Get(i));
            }
        }

        [Fact]
        public void ComparisonTest()
        {
            var bitArray1 = new BitArray(2);
            bitArray1.Set(0, true);
            var bitArray2 = new BitArray(2);
            bitArray2.Set(0, true);
            Assert.Equal(bitArray1, bitArray2);
            Assert.Equal(bitArray1.GetHashCode(), bitArray2.GetHashCode());
            Assert.Equal(bitArray1.ToString(), bitArray2.ToString());
        }
    }
}
