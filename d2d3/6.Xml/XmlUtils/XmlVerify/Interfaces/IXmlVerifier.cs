using System.IO;
using XmlVerify.Models;

namespace XmlVerify.Interfaces
{
    public interface IXmlVerifier
    {
        XmlCheckResult CheckXml(Stream xmlStream, bool closeStreamOnFinish = false);
    }
}