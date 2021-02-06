using Scriber.Layout;
using Scriber.Text;
using System;
using Xunit;

namespace Scriber.Engine.Converter.Tests
{
    public class StringConverterTests
    {
        private readonly StringConverter converter = new StringConverter();

        [Fact]
        public void NullValueException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                converter.Convert(null, typeof(int));
            });
            Assert.Equal("source", ex.ParamName);
        }

        [Fact]
        public void NullTypeException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                converter.Convert("notNull", null);
            });
            Assert.Equal("targetType", ex.ParamName);
        }

        [Theory]
        [InlineData(32, "32", typeof(int))]
        [InlineData(-32, "-32", typeof(int))]
        [InlineData(32u, "32", typeof(uint))]
        [InlineData((short)32, "32", typeof(short))]
        [InlineData((short)-32, "-32", typeof(short))]
        [InlineData((ushort)32, "32", typeof(ushort))]
        [InlineData((long)32, "32", typeof(long))]
        [InlineData((long)-32, "-32", typeof(long))]
        [InlineData((ulong)32u, "32", typeof(ulong))]
        [InlineData((byte)32, "32", typeof(byte))]
        [InlineData((sbyte)-32, "-32", typeof(sbyte))]
        [InlineData(32.0, "32", typeof(double))]
        [InlineData(32.4, "32.4", typeof(double))]
        [InlineData(32.32f, "32.32", typeof(float))]
        [InlineData(true, "TRUE", typeof(bool))]
        [InlineData(true, "True", typeof(bool))]
        [InlineData(false, "false", typeof(bool))]
        [InlineData(FontWeight.Bold, "bold", typeof(FontWeight))]
        [InlineData(FontWeight.Italic, "2", typeof(FontWeight))]
        public void StringToValueSuccess(object expected, string value, Type type)
        {
            var converted = converter.Convert(value, type);

            Assert.IsType(type, converted);
            Assert.Equal(expected, converted);
        }

        [Fact]
        public void StringToDecimalSuccess()
        {
            var converted = converter.Convert("32.4", typeof(decimal));

            var dec = Assert.IsType<decimal>(converted);
            Assert.Equal(32.4m, dec);
        }

        [Theory]
        [InlineData("2pt", 2, 2, 2, 2)]
        [InlineData("2pt 3pt", 2, 3, 2, 3)]
        [InlineData("2pt 4 5pt 6pt", 2, 4, 5, 6)]
        public void StringToThicknessSuccess(string input, double top, double left, double bottom, double right)
        {
            var thickness = converter.Convert(input, typeof(Thickness));

            var value = Assert.IsType<Thickness>(thickness);
            Assert.Equal(top, value.Top, 3);
            Assert.Equal(left, value.Left, 3);
            Assert.Equal(bottom, value.Bottom, 3);
            Assert.Equal(right, value.Right, 3);
        }

        [Fact]
        public void StringToUnitSuccess()
        {
            var converted = converter.Convert("32.4cm", typeof(Unit));

            var unit = Assert.IsType<Unit>(converted);
            Assert.Equal(new Unit(32.4, UnitType.Centimeter), unit);
        }

        [Theory]
        [InlineData("0", 5, 0)]
        [InlineData("2", 5, 2)]
        [InlineData("^0", 5, 5)]
        [InlineData("^2", 5, 3)]
        public void StringToIndexSuccess(string input, int length, int resultIndex)
        {
            var converted = converter.Convert(input, typeof(Index));

            var index = Assert.IsType<Index>(converted);
            Assert.Equal(resultIndex, index.GetOffset(length));
        }

        [Theory]
        [InlineData("0", 5, 0, 0)]
        [InlineData("2-3", 5, 2, 1)]
        [InlineData("^4-^2", 5, 1, 2)]
        [InlineData("0-^1", 5, 0, 4)]
        public void StringToRangeSuccess(string input, int rangeSize, int offset, int length)
        {
            var converted = converter.Convert(input, typeof(Range));

            var range = Assert.IsType<Range>(converted);
            Assert.Equal((offset, length), range.GetOffsetAndLength(rangeSize));
        }

        private const string TooLargeForLong = "92233720368547758070";
        private const string NaN = "not a number";

        [Theory]
        [InlineData(TooLargeForLong, typeof(int))]
        [InlineData(TooLargeForLong, typeof(short))]
        [InlineData(TooLargeForLong, typeof(byte))]
        [InlineData(TooLargeForLong, typeof(long))]
        [InlineData(TooLargeForLong, typeof(uint))]
        [InlineData(TooLargeForLong, typeof(ushort))]
        [InlineData(TooLargeForLong, typeof(sbyte))]
        [InlineData(TooLargeForLong, typeof(ulong))]
        [InlineData(NaN, typeof(int))]
        [InlineData(NaN, typeof(short))]
        [InlineData(NaN, typeof(byte))]
        [InlineData(NaN, typeof(long))]
        [InlineData(NaN, typeof(uint))]
        [InlineData(NaN, typeof(ushort))]
        [InlineData(NaN, typeof(sbyte))]
        [InlineData(NaN, typeof(ulong))]
        [InlineData(NaN, typeof(decimal))]
        [InlineData(NaN, typeof(float))]
        [InlineData(NaN, typeof(double))]
        [InlineData("yes", typeof(bool))]
        [InlineData("no", typeof(bool))]
        [InlineData("none", typeof(FontWeight))]
        [InlineData("unknown", typeof(Array))]
        public void StringConversionFailure(string value, Type type)
        {
            Assert.ThrowsAny<Exception>(() =>
            {
                converter.Convert(value, type);
            });
        }
    }
}
