using Scriber.Text;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Scriber.Util.Tests
{
    public class EnumUtilityTests
    {
        [Fact]
        public void Parse()
        {
            var enumValue = EnumUtility.ParseEnum<FontWeight>("Bold");
            Assert.Equal(FontWeight.Bold, enumValue);
        }

        [Fact]
        public void ParseIgnoreWhitespace()
        {
            var enumValue = EnumUtility.ParseEnum<FontWeight>("  Bold  \t");
            Assert.Equal(FontWeight.Bold, enumValue);
        }

        [Fact]
        public void ParseIgnoreCase()
        {
            var enumValue = EnumUtility.ParseEnum<FontWeight>("BOLD");
            Assert.Equal(FontWeight.Bold, enumValue);
        }

        [Fact]
        public void ParseNonExisting()
        {
            var enumValue = EnumUtility.ParseEnum<FontWeight>("none");
            Assert.Null(enumValue);
        }

        [Fact]
        public void TryParseSuccess()
        {
            var result = EnumUtility.TryParseEnum<FontWeight>("bold", out var output);
            Assert.True(result);
            Assert.Equal(FontWeight.Bold, output);
        }

        [Fact]
        public void TryParseFailure()
        {
            var result = EnumUtility.TryParseEnum<FontWeight>("none", out var output);
            Assert.False(result);
        }

        [Fact]
        public void GetNameSuccess()
        {
            var name = EnumUtility.GetName(FontWeight.Bold);
            Assert.Equal("Bold", name);
        }

        [Fact]
        public void GetNameFailure()
        {
            var name = EnumUtility.GetName((FontWeight)999);
            Assert.Null(name);
        }
    }
}
