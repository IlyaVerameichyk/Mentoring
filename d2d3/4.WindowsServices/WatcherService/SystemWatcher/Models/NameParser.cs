using System.CodeDom.Compiler;
using System.Text.RegularExpressions;

namespace SystemWatcher.Models
{
    public class NameParser
    {
        private const string RegexTemplate = "(?<prefix>[a-zA-Z]+)_(?<number>\\d+).(?<extension>[a-zA-Z]+)";
        private string _originalName;
        private Regex _regex;

        public NameParser(string originalName)
        {
            _originalName = originalName;
            _regex = new Regex(RegexTemplate);
        }

        public string Prefix => _regex.Match(_originalName).Groups["prefix"].Value;
        public int Number => int.Parse(_regex.Match(_originalName).Groups["number"].Value);
        public string Extension => _regex.Match(_originalName).Groups["extension"].Value;
    }
}