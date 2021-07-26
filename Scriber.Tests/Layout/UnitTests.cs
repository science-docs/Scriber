using System.ComponentModel;
using System.Globalization;
using Xunit;

namespace Scriber.Layout.Tests
{
    public class UnitTests
    {
        [Theory]
        [InlineData("32.4cm", 32.4, UnitType.Centimeter)]
        [InlineData("32pt", 32, UnitType.Point)]
        [InlineData("32", 32, UnitType.Point)]
        [InlineData("32\tpx", 32, UnitType.Presentation)]
        [InlineData("32 mm", 32, UnitType.Millimeter)]
        [InlineData("32 in", 32, UnitType.Inch)]
        [InlineData("96 / 3 mm", 32, UnitType.Millimeter)]
        public void ParseSuccess(string input, double value, UnitType type)
        {
            Assert.True(Unit.TryParse(input, out var unit));
            Assert.Equal(value, unit.Value, 3);
            Assert.Equal(type, unit.Type);
        }

        [Theory]
        [InlineData("32,4cm")]
        [InlineData("pt")]
        [InlineData("32ppt")]
        [InlineData("32inch")]
        [InlineData("_px")]
        public void ParseFailure(string input)
        {
            Assert.False(Unit.TryParse(input, out var _));
        }

        [Fact]
        public void CreateDefault()
        {
            var unit = new Unit(24);
            Assert.Equal(24, unit.Value);
            Assert.Equal(UnitType.Point, unit.Type);
        }

        [Fact]
        public void CreateWithException()
        {
            var ex = Assert.Throws<InvalidEnumArgumentException>(() =>
            {
                new Unit(24, (UnitType)99);
            });
            Assert.Equal("type", ex.ParamName);
        }

        [Fact]
        public void FromPoint()
        {
            var unit = Unit.FromPoint(24);
            Assert.Equal(24, unit.Value);
            Assert.Equal(24, unit.Point);
            Assert.Equal(UnitType.Point, unit.Type);
        }

        [Fact]
        public void FromInch()
        {
            var unit = Unit.FromInch(24);
            Assert.Equal(24, unit.Value);
            Assert.Equal(24, unit.Inch);
            Assert.Equal(UnitType.Inch, unit.Type);
        }

        [Fact]
        public void FromMillimeter()
        {
            var unit = Unit.FromMillimeter(24);
            Assert.Equal(24, unit.Value);
            Assert.Equal(24, unit.Millimeter);
            Assert.Equal(UnitType.Millimeter, unit.Type);
        }

        [Fact]
        public void FromCentimeter()
        {
            var unit = Unit.FromCentimeter(24);
            Assert.Equal(24, unit.Value);
            Assert.Equal(24, unit.Centimeter);
            Assert.Equal(UnitType.Centimeter, unit.Type);
        }

        [Fact]
        public void FromPresentation()
        {
            var unit = Unit.FromPresentation(24);
            Assert.Equal(24, unit.Value);
            Assert.Equal(24, unit.Presentation);
            Assert.Equal(UnitType.Presentation, unit.Type);
        }

        [Fact]
        public void ToStringDefault()
        {
            var unit = new Unit(24.5, UnitType.Point);
            Assert.Equal("24.5pt", unit.ToString());
        }

        [Fact]
        public void ToStringWithFormatProvider()
        {
            var unit = new Unit(24.5, UnitType.Point);
            Assert.Equal("24,5pt", unit.ToString(new CultureInfo("de-DE")));
        }

        [Fact]
        public void ToStringWithFormatAndProvider()
        {
            var unit = new Unit(24.5, UnitType.Point);
            Assert.Equal("24,500pt", unit.ToString("0.000", new CultureInfo("de-DE")));
        }

        [Fact]
        public void ComparisonEqual()
        {
            var unit1 = new Unit(24.5, UnitType.Point);
            var unit2 = new Unit(24.5, UnitType.Point);
            Assert.True(unit1 == unit2);
            Assert.Equal(unit1.GetHashCode(), unit2.GetHashCode());
        }

        [Fact]
        public void ComparisonNotEqual()
        {
            var unit1 = new Unit(24.5, UnitType.Point);
            var unit2 = new Unit(24.5, UnitType.Centimeter);
            Assert.True(unit1 != unit2);
            Assert.NotEqual(unit1.GetHashCode(), unit2.GetHashCode());
        }
    }
}
