using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Scriber.Engine.Tests
{
    public class DynamicDictionaryTests
    {
        [Fact]
        public void SetAndRetrieveValue()
        {
            dynamic obj = new DynamicDictionary();
            obj.key = "value";
            Assert.IsType<string>(obj.key);
            Assert.Equal("value", obj.key);
        }

        [Fact]
        public void CaseInsensitiveFields()
        {
            dynamic obj = new DynamicDictionary();
            obj.KEY = "value";
            Assert.Equal("value", obj.key);
        }
    }
}
