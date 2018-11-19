using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace XmlToRssGenerator
{
    public class XslTransformer
    {
        public static readonly string RssXslPath = "Resources\\RssStylesheet.xslt";
        public static readonly string HtmlXslPath = "Resources\\HtmlStylesheet.xslt";

        public Stream TransformToRss(Stream xmlStream, bool closeStreamOnFinish = false)
        {
            if (xmlStream == null)
            {
                throw new ArgumentNullException(nameof(xmlStream));
            }
            try
            {
                var result = new MemoryStream();
                var xslt = new XslTransform();
                xslt.Load(RssXslPath);
                var xPathDocument = new XPathDocument(xmlStream);
                var settings = new XmlWriterSettings()
                {
                    OmitXmlDeclaration = false,
                    Encoding = Encoding.Unicode
                };
                var writer = XmlWriter.Create(result, settings);
                xslt.Transform(xPathDocument, null, writer, null);
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

        public Stream TransformToHtml(Stream xmlStream, bool closeStreamOnFinish = false)
        {
            if (xmlStream == null)
            {
                throw new ArgumentNullException(nameof(xmlStream));
            }
            try
            {
                var result = new MemoryStream();
                var xslt = new XslTransform();
                xslt.Load(HtmlXslPath);
                var xPathDocument = new XPathDocument(xmlStream);
                var settings = new XmlWriterSettings()
                {
                    OmitXmlDeclaration = true,
                    Encoding = Encoding.Unicode,
                    ConformanceLevel = ConformanceLevel.Auto
                };
                var writer = XmlWriter.Create(result, settings);
                xslt.Transform(xPathDocument, null, writer, null);
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
    }
}
