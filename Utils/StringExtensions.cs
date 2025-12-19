using System.Text.RegularExpressions;

namespace RFRocketLibrary.Utils
{
    public static class StringExtensions
    {
        #region Methods

        public static string RemoveRichTag(this string text)
        {
            return Regex.Replace(text, "<[^>]+>", string.Empty);;
        }

        public static string SplitCamelCase(this string text, string separator = " ")
        {
            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
            return r.Replace(text, separator);
        }

        #endregion
    }
}