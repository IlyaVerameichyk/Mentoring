using System.Collections.Generic;

namespace XmlVerify.Models
{
    public class XmlCheckResult
    {
        public bool IsValid { get; private set; }

        public IEnumerable<string> Violations { get; }

        public XmlCheckResult()
        {
            IsValid = true;
            Violations = new List<string>();
        }

        public void AddViolation(string description)
        {
            IsValid = false;
            ((List<string>)Violations).Add(description);
        }
    }
}