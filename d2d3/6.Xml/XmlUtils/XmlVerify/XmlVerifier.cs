using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using XmlVerify.Interfaces;
using XmlVerify.Models;

namespace XmlVerify
{
    public class XmlVerifier : IXmlVerifier
    {
        private readonly IXmlValidationMessageFormatter _messageFormatter;

        public XmlVerifier(IXmlValidationMessageFormatter messageFormatter)
        {
            _messageFormatter = messageFormatter;
        }

        public XmlCheckResult CheckXml(Stream xmlStream, bool closeStreamOnFinish = false)
        {
            if (xmlStream == null)
            {
                throw new ArgumentNullException(nameof(xmlStream));
            }
            try
            {
                var result = new XmlCheckResult();
                var xmlReaderSettings = PrepareSettings(result);
                var xmlReader = XmlReader.Create(xmlStream, xmlReaderSettings);
                while (xmlReader.Read())
                {
                }
                return result;
            }
            finally
            {
                if (closeStreamOnFinish)
                {
                    xmlStream.Close();
                }
            }
        }

        private XmlReaderSettings PrepareSettings(XmlCheckResult checkResult)
        {
            var settings = new XmlReaderSettings();
            settings.Schemas.Add("http://library.by/catalog", Path.Combine("Resources", "books.xsd"));
            settings.ValidationEventHandler += (sender, args) =>
            {
                var lineInfo = sender as IXmlLineInfo;
                checkResult.AddViolation(_messageFormatter.FormatMessage(lineInfo, args.Message));
            };
            settings.ValidationFlags = settings.ValidationFlags | XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationType = ValidationType.Schema;
            return settings;
        }
    }
}