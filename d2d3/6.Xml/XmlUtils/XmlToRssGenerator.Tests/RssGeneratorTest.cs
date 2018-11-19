using System;
using System.IO;
using System.Text;
using System.Xml;
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
        public void TestTransformToRss_GenerateRss_AssertResult()
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
                using (var actual = _generator.TransformToRss(fs))
                {
                    var actualString = Encoding.Unicode.GetString(((MemoryStream)actual).ToArray());

                    Assert.Equal(expectedString.Replace("\r\n", "").Replace(" ", ""), actualString.Replace(" ", "").Replace("\r\n", ""), StringComparer.InvariantCultureIgnoreCase);
                }
            }
        }

        [Fact]
        public void TestTransformToRss_PassNotXml_MustThrowXmlException()
        {
            using (var ms = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 }))
            {
                Assert.Throws<XmlException>(() => _generator.TransformToRss(ms));
            }
        }

        [Fact]
        public void TestTransformToRss_PassNull_MustThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _generator.TransformToRss(null));
        }

        [Fact]
        public void TestTransformToHtml_GenerateHtml_AssertResult()
        {
            using (var fs = new FileStream("Resources\\books.xml", FileMode.Open))
            {
                using (var actual = _generator.TransformToHtml(fs))
                {
                    var actualString = Encoding.Unicode.GetString(((MemoryStream)actual).ToArray());

                    Assert.False(string.IsNullOrWhiteSpace(actualString));
                }
            }
        }

        [Fact]
        public void TestTransformToHtml_PassNotXml_MustThrowXmlException()
        {
            using (var ms = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 }))
            {
                Assert.Throws<XmlException>(() => _generator.TransformToHtml(ms));
            }
        }

        [Fact]
        public void TestTransformToHtml_PassNull_MustThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _generator.TransformToHtml(null));
        }
    }
}
