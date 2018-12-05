using System;
using System.IO;
using System.Linq;
using System.Xml;
using Moq;
using XmlVerify.Interfaces;
using Xunit;

namespace XmlVerify.Tests
{
    public class XmlVerifierTest
    {
        private readonly Mock<IXmlValidationMessageFormatter> _mock;
        private readonly XmlVerifier _verifier;

        public XmlVerifierTest()
        {
            var mock = new Mock<IXmlValidationMessageFormatter>();
            mock.Setup(m => m.FormatMessage(It.IsAny<IXmlLineInfo>(), It.IsAny<string>())).Returns("string");
            _mock = mock;

            _verifier = new XmlVerifier(mock.Object);
        }

        [Theory]
        [InlineData("books.xml", true)]
        [InlineData("incorrect.xml", false)]
        [InlineData("nonUniqueId.xml", false)]
        public void CheckXml_CheckXmlFiles_AssertResult(string fileName, bool expectedVerifyResult)
        {
            using (var fs = new FileStream(Path.Combine("Resources", fileName), FileMode.Open))
            {
                var actual = _verifier.CheckXml(fs);

                Assert.Equal(expectedVerifyResult, actual.IsValid);
            }
        }

        [Fact]
        public void CheckXml_PassNull_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _verifier.CheckXml(null));
        }

        [Fact]
        public void CheckXml_PassCloseStream_ShouldCloseStream()
        {
            var fs = new FileStream(Path.Combine("Resources", "books.xml"), FileMode.Open);

            var actual = _verifier.CheckXml(fs, true);

            Assert.False(fs.CanRead);
        }

        [Fact]
        public void CheckXml_AssertValidationMessage_ShouldCallFormatMessage()
        {
            using (var fs = new FileStream(Path.Combine("Resources", "incorrect.xml"), FileMode.Open))
            {
                var actual = _verifier.CheckXml(fs);

                Assert.Single(actual.Violations);
                _mock.Verify(m => m.FormatMessage(It.IsAny<IXmlLineInfo>(), It.IsAny<string>()));
            }
        }
    }
}
