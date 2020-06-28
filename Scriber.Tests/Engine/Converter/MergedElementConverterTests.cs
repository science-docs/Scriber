using Scriber.Layout.Document;
using Xunit;

namespace Scriber.Engine.Converter.Tests
{
    public class MergedElementConverterTests
    {
        [Fact]
        public void ParagraphToInt()
        {
            var paragraph = Paragraph.FromText("123");
            var mergedConverter = new MergedElementConverter(new ParagraphConverter());
            mergedConverter.Add(typeof(string), new StringConverter());

            var value = mergedConverter.Convert(paragraph, typeof(int));
            Assert.IsType<int>(value);
            var intValue = (int)value;
            Assert.Equal(123, intValue);
        }
    }
}
