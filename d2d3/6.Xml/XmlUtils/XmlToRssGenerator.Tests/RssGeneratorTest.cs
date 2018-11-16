using System;
using System.IO;
using System.Text;
using Xunit;

namespace XmlToRssGenerator.Tests
{
    public class RssGeneratorTest
    {
        private readonly XslTransformer _generator;

        public RssGeneratorTest()
        {
            _generator = new XslTransformer();
        }

        [Fact]
        public void TestTransform_GenerateRss_AssertResult()
        {
            string expectedString;
            using (var expected = new FileStream("Resources\\expectedRss.xml", FileMode.Open))
            using (var ms = new MemoryStream())
            {
                expected.CopyTo(ms);
                expectedString = Encoding.UTF8.GetString(ms.ToArray());
            }

            using (var fs = new FileStream("Resources\\books.xml", FileMode.Open))
            {
                using (var actual = _generator.TransformToRss(XslTransformer.RssXslPath, fs))
                {
                    var actualString = Encoding.Unicode.GetString(((MemoryStream)actual).ToArray());

                    Assert.Equal(expectedString.Replace("\r\n", "").Replace(" ", ""), actualString.Replace(" ", "").Replace("\r\n", ""), StringComparer.InvariantCultureIgnoreCase);
                }
            }
        }

        [Fact]
        public void TestTransform_GenerateHeml_AssertResult()
        {
            string expectedString;
            using (var expected = new FileStream("Resources\\expectedHtml.html", FileMode.Open))
            using (var ms = new MemoryStream())
            {
                expected.CopyTo(ms);
                expectedString = Encoding.UTF8.GetString(ms.ToArray());
            }

            using (var fs = new FileStream("Resources\\books.xml", FileMode.Open))
            {
                using (var actual = _generator.TransformToHtml(XslTransformer.HtmlXslPath, fs))
                {
                    var actualString = Encoding.Unicode.GetString(((MemoryStream)actual).ToArray());

                    Assert.Equal(expectedString.Replace("\r\n", "").Replace(" ", ""), actualString.Replace(" ", "").Replace("\r\n", ""), StringComparer.InvariantCultureIgnoreCase);
                }
            }
        }
    }
}
