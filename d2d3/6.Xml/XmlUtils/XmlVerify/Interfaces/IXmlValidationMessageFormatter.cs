using System.Xml;

namespace XmlVerify.Interfaces
{
    public interface IXmlValidationMessageFormatter
    {
        string FormatMessage(IXmlLineInfo lineInfo,string message);
    }
}