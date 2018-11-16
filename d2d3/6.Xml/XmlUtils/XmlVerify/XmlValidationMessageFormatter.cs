using System;
using System.Xml;
using XmlVerify.Interfaces;

namespace XmlVerify
{
    public class XmlValidationMessageFormatter : IXmlValidationMessageFormatter
    {
        public string FormatMessage(IXmlLineInfo lineInfo, string message)
        {
            return $"Element at line {lineInfo.LineNumber}, position {lineInfo.LinePosition} has violation.{Environment.NewLine}" +
                   $"Detailed message: {message}";
        }
    }
}