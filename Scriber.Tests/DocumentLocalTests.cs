using System;
using System.Collections.Generic;
using Xunit;

namespace Scriber.Tests
{
    public class DocumentLocalTests
    {
        private static Document Document { get; } = new Document();

        [Fact]
        public void CreatedWithConstantConsumer()
        {
            var local = new DocumentLocal<int>(4);
            var value = local.Get(Document);
            Assert.Equal(4, value);
        }

        [Fact]
        public void CreatedWithDynamicConsumer()
        {
            var local = new DocumentLocal<List<object>>(() => new List<object>());
            var value = local.Get(Document);
            Assert.NotNull(value);
            Assert.Empty(value);
        }

        [Fact]
        public void GetterSuccess()
        {
            var local = new DocumentLocal<List<object>>(() => new List<object>());
            var value = local[Document];
            value.Add(2);
            var again = local[Document];
            Assert.Equal(again, value);
            Assert.Equal(2, again[0]);
        }

        [Fact]
        public void SetterSuccess()
        {
            var local = new DocumentLocal<int>(0);
            local[Document] = 2;
            var value = local[Document];
            Assert.Equal(2, value);
        }

        [Fact]
        public void PerDocumentValue()
        {
            var local = new DocumentLocal<int>(0);
            local[Document] = 5;
            var doc = new Document();
            local[doc] = 10;

            Assert.Equal(5, local[Document]);
            Assert.Equal(10, local[doc]);
        }

        [Fact]
        public void NullDefaultValue()
        {
            var local = new DocumentLocal<object>((object)null);
            Assert.Null(local.Get(new Document()));
        }

        [Fact]
        public void NonValueTypeValue()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                new DocumentLocal<Type>(typeof(DocumentLocal<>));
            });
            Assert.Equal("defaultValue", ex.ParamName);
        }

        [Fact]
        public void NullConsumer()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                new DocumentLocal<object>(null);
            });
            Assert.Equal("consumer", ex.ParamName);
        }

        [Fact]
        public void NullConsumerValue()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                new DocumentLocal<object>(() => null);
            });
            Assert.Equal("consumer", ex.ParamName);
        }
    }
}
